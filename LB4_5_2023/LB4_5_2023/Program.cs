using System;
using System.Threading;

class Program
{
    static int[,] matrixA = { { 1, 4 }, { 2, 5 }, { 3, 6 } };
    static int[,] matrixB = { { 8, 7, 6 }, { 5, 4, 3 } };
    static int[,] result = new int[matrixA.GetLength(0), matrixB.GetLength(1)];
    static int maxThreads = 2;
    static Semaphore semaphore = new Semaphore(maxThreads, maxThreads);
    static object lockObject = new object();

    static void Main(string[] args)
    {
        Thread[] threads = new Thread[matrixA.GetLength(0)];
        for (int i = 0; i < matrixA.GetLength(0); i++)
        {
            threads[i] = new Thread(new ParameterizedThreadStart(Multiply));
            threads[i].Start(i);
        }
        for (int i = 0; i < matrixA.GetLength(0); i++)
        {
            threads[i].Join();
        }
        PrintResult();
    }

    static void Multiply(object index)
    {
        int i = (int)index;
        semaphore.WaitOne();
        for (int j = 0; j < matrixB.GetLength(1); j++)
        {
            int sum = 0;
            for (int k = 0; k < matrixA.GetLength(1); k++)
            {
                sum += matrixA[i, k] * matrixB[k, j];
            }
            lock (lockObject)
            {
                result[i, j] = sum;
            }
        }
        semaphore.Release();
    }

    static void PrintResult()
    {
        for (int i = 0; i < result.GetLength(0); i++)
        {
            for (int j = 0; j < result.GetLength(1); j++)
            {
                Console.Write(result[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}

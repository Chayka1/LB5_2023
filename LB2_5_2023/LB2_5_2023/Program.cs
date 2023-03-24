using System;
using System.Threading;

class Program
{
    static Semaphore semaphore = new Semaphore(2, 2);
    static object lockObject = new object();
    static int currentLight = 0;
    static int[] lightsTimings = new int[] { 1000, 2000, 3000, 4000 };
    static Thread[] lights = new Thread[4];

    static void Main(string[] args)
    {
        for (int i = 0; i < 4; i++)
        {
            lights[i] = new Thread(() =>
            {
                while (true)
                {
                    lock (lockObject)
                    {
                        if (currentLight == i)
                        {
                            Console.WriteLine($"Light {i + 1} is green.");
                            semaphore.WaitOne();
                            Thread.Sleep(5000);
                            semaphore.Release();
                            Console.WriteLine($"Light {i + 1} is red.");
                            currentLight = (currentLight + 1) % 4;
                        }
                        else
                        {
                            Console.WriteLine($"Light {i + 1} is red.");
                            Thread.Sleep(lightsTimings[i]);
                        }
                    }
                }
            });
            lights[i].Start();
        }

        Console.ReadKey();
    }
}

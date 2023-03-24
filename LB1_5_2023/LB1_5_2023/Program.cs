using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumerExample
{
    class Program
    {
        static Queue<int> queue = new Queue<int>();
        static object lockObject = new object();

        static void Main(string[] args)
        {
            Thread producerThread = new Thread(Producer);
            Thread consumerThread = new Thread(Consumer);

            producerThread.Start();
            consumerThread.Start();

            producerThread.Join();
            consumerThread.Join();
        }

        static void Producer()
        {
            Random random = new Random();
            while (true)
            {
                int number = random.Next(100);
                lock (lockObject)
                {
                    queue.Enqueue(number);
                    Console.WriteLine($"Produced number {number}");
                }
                Thread.Sleep(1000);
            }
        }

        static void Consumer()
        {
            while (true)
            {
                int number;
                lock (lockObject)
                {
                    if (queue.Count == 0)
                    {
                        continue;
                    }
                    number = queue.Dequeue();
                }
                Console.WriteLine($"Consumed number {number}");
                Thread.Sleep(500);
            }
        }
    }
}

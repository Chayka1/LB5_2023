using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int[] array = new int[] { 3, 7, 1, 9, 2, 6, 8, 4, 5 };

        Console.WriteLine("Input array: [{0}]", string.Join(", ", array));

        QuickSortParallel(array, 0, array.Length - 1);

        Console.WriteLine("Sorted array: [{0}]", string.Join(", ", array));
    }

    static void QuickSortParallel(int[] array, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(array, left, right);

            // Launch two new threads for sorting the left and right subarrays in parallel
            Thread leftThread = new Thread(() => QuickSortParallel(array, left, pivot - 1));
            Thread rightThread = new Thread(() => QuickSortParallel(array, pivot + 1, right));

            leftThread.Start();
            rightThread.Start();

            leftThread.Join();
            rightThread.Join();
        }
    }

    static int Partition(int[] array, int left, int right)
    {
        int pivot = array[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (array[j] < pivot)
            {
                i++;
                Swap(ref array[i], ref array[j]);
            }
        }

        Swap(ref array[i + 1], ref array[right]);
        return i + 1;
    }

    static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }
}

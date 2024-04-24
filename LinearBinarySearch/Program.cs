using System.Diagnostics;

namespace LinearBinarySearch
{
    public class LinearBinary
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Message \t \t Size \t Mean \t\t\t Standard deviation \t Count");

            for (int size = 10; size < 10_000_000; size *= 2) 
            {
                int[] intArray = FillIntArray(size); // sorted [0,1,...]
                int[] items = FillIntArray(size);
                int n = size;
                Random random = new Random();
                random.Shuffle(intArray);
                benchmark("binary_search_success", $"{size}",
                    i=>Binary(intArray, items[i % n]), 10, 0.25);
            }

            Console.WriteLine();
            Console.WriteLine("Message \t \t Size \t Mean \t\t\t Standard deviation \t Count");

            for (int size = 10; size < 10_000_000; size *= 2)
            {
                int[] intArray = FillIntArray(size); // sorted [0,1,...]
                int[] items = FillIntArray(size);
                int n = size;
                Random random = new Random();
                random.Shuffle(intArray);
                benchmark("linear_search_success", $"{size}",
                    i => Linear(intArray, items[i % n]), 10, 0.25);
            }
        }

        public static int[] FillIntArray(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = i;
            }
            return array;
        }

        public static double benchmark(String msg, String size, Func<int, double> f, int n, double minTime)
        {
            int count = 1, totalCount = 0;
            double dummy = 0.0, runningTime = 0.0, st = 0.0, sst = 0.0;
            do
            {
                count *= 2;
                st = sst = 0.0;
                for (int j = 0; j < n; j++)
                {
                    Timer t = new Timer();
                    for (int i = 0; i < count; i++)
                        dummy += f.Invoke(i);
                    runningTime = t.Check();
                    double time = runningTime * 1e9 / count;
                    st += time;
                    sst += time * time;
                    totalCount += count;
                }
            } while (runningTime < minTime && count < int.MaxValue / 2);
            double mean = st / n, sdev = Math.Sqrt((sst - mean * mean * n) / (n - 1));
            Console.WriteLine($"{msg} \t {size} \t {mean} \t {sdev} \t {count}");
            return dummy / totalCount;
        }

        public static double Linear(int[] array, int x)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == x)
                    return i;
            }
            return -1;
        }

        public static double Binary(int[] array, int x)
        {
            int start = 0;
            int end = array.Length - 1;
            while (start <= end)
            {
                int mid = (start + end) / 2;
                if (x == array[mid])
                {
                    return mid;
                }
                else if (x > array[mid])
                {
                    start = mid + 1;
                }
                else
                {
                    end = mid - 1;
                }
            }
            return -1;
        }
    }
    public class Timer
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        public Timer() { Play(); }
        public double Check() { return stopwatch.ElapsedMilliseconds / 1000.0; }
        public void Pause() { stopwatch.Stop(); }
        public void Play() { stopwatch.Start(); }
    }
}
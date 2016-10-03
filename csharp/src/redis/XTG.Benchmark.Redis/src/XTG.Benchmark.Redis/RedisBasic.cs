using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTG.BarceloHotels.Common.Redis.Parse;

namespace XTG.Benchmark.Redis
{
    public class RedisBasic
    {
        public static void WriteRedisHash()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number Keys: ");
            Console.ResetColor();
            string keys = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number HashKeys: ");
            Console.ResetColor();
            string hashkeys = Console.ReadLine();

            int nkey = Convert.ToInt32(keys);
            int nhashkeys = Convert.ToInt32(hashkeys);

            Stopwatch clock = new Stopwatch();
            clock.Start();
            Dictionary<string, Dictionary<string, string>> ret = new Dictionary<string, Dictionary<string, string>>();
            for (int i = 0; i < nkey; i++)
            {
                ret.Add(i.ToString(), new Dictionary<string, string>());
                for (int j = 0; j < nhashkeys; j++)
                {
                    ret[i.ToString()].Add(j.ToString(), j.ToString());
                }
            }
            clock.Stop();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.Write(String.Format("Time build: {0} ms", clock.ElapsedMilliseconds));

            Stopwatch clock2 = new Stopwatch();
            clock2.Start();
            var b = RedisDataConnection.Instance.HashSetAsync(ret).Result;
            clock2.Stop();
            Console.WriteLine();
            Console.Write(String.Format("Time write: {0} ms", clock2.ElapsedMilliseconds));
            Console.WriteLine();
            Console.Write(String.Format("Total: {0} ms \n \n", clock.ElapsedMilliseconds + clock2.ElapsedMilliseconds));
            Console.WriteLine("End \n");
            Console.ResetColor();
        }

        public static void WriteRedisString()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number Keys: ");
            Console.ResetColor();
            string keys = Console.ReadLine();

            int nkey = Convert.ToInt32(keys);

            Stopwatch clock = new Stopwatch();
            clock.Start();
            Dictionary<string, string> ret = new Dictionary<string, string>();
            for (int i = 0; i < nkey; i++)
            {
                ret.Add(i.ToString(), i.ToString());
            }
            clock.Stop();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.Write(String.Format("Time build: {0} ms", clock.ElapsedMilliseconds));

            Stopwatch clock2 = new Stopwatch();
            clock2.Start();
            var b = RedisDataConnection.Instance.StringSetAsync(ret).Result;
            clock2.Stop();
            Console.WriteLine();
            Console.Write(String.Format("Time write: {0} ms", clock2.ElapsedMilliseconds));
            Console.WriteLine();
            Console.Write(String.Format("Total: {0} ms \n \n", clock.ElapsedMilliseconds + clock2.ElapsedMilliseconds));
            Console.WriteLine("End \n");
            Console.ResetColor();
        }

        public static async Task ReadRedisHash()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number Keys: ");
            Console.ResetColor();
            string keys = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number HashKeys: ");
            Console.ResetColor();
            string hashkeys = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number iterations: ");
            Console.ResetColor();
            string iterations = Console.ReadLine();

            int nkey = Convert.ToInt32(keys);
            int nhashkeys = Convert.ToInt32(hashkeys);
            int niterations = Convert.ToInt32(iterations);

            Stopwatch clock = new Stopwatch();
            clock.Start();
            Dictionary<string, HashSet<string>> ret = new Dictionary<string, HashSet<string>>();
            for (int i = 0; i < nkey; i++)
            {
                ret.Add(i.ToString(), new HashSet<string>());
                for (int j = 0; j < nhashkeys; j++)
                {
                    ret[i.ToString()].Add(j.ToString());
                }
            }
            clock.Stop();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.Write(String.Format("Time build: {0} ms", clock.ElapsedMilliseconds));

            Stopwatch clock2 = new Stopwatch();
            for (int i = 0; i < niterations; i++)
            {
                clock2.Restart();
                await RedisDataConnection.Instance.HashGetAsync(ret);
                clock2.Stop();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine();
                Console.Write(String.Format("Time response {1}: {0} ms", clock2.ElapsedMilliseconds, i));
            }
            Console.WriteLine(" \nEnd \n");
        }

        public static async Task ReadRedisString()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number Keys: ");
            Console.ResetColor();
            string keys = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number iterations: ");
            Console.ResetColor();
            string iterations = Console.ReadLine();

            int nkey = Convert.ToInt32(keys);
            int niterations = Convert.ToInt32(iterations);

            Stopwatch clock = new Stopwatch();
            clock.Start();
            HashSet<string> ret = new HashSet<string>();
            for (int i = 0; i < nkey; i++)
            {
                ret.Add(i.ToString());
            }
            clock.Stop();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.Write(String.Format("Time build: {0} ms", clock.ElapsedMilliseconds));

            Stopwatch clock2 = new Stopwatch();
            for (int i = 0; i < niterations; i++)
            {
                clock2.Restart();
                await RedisDataConnection.Instance.StringGetAsync(ret.ToArray());
                clock2.Stop();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine();
                Console.Write(String.Format("Time response {1}: {0} ms", clock2.ElapsedMilliseconds, i));
            }
            Console.WriteLine(" \nEnd \n");
        }


        public static async Task TestReadRedisString()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number strings: ");
            Console.ResetColor();
            string nstrings = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Arrays number threads: ");
            Console.ResetColor();
            string threads = Console.ReadLine();

            int inumbstrings = Convert.ToInt32(nstrings);
            List<string> nthreads = threads.Split(',').ToList();

            HashSet<string> ret = new HashSet<string>();
            for (int i = 0; i < inumbstrings; i++)
            {
                ret.Add(i.ToString());
            }

            RedisKey[] keys = ParseTo.RedisKey(ret.ToArray()).ToArray();
            Stopwatch clock = new Stopwatch();
            Dictionary<int, int> results = new Dictionary<int, int>();

            await RedisDataConnection.Instance.StringGetAsync(keys);

            Console.Write("Run Test...");

            foreach (string numberThreads in nthreads)
            {
                int numeroThreads = Convert.ToInt32(numberThreads);
                int ops_seg = 0;
                clock.Restart();
                List<Task<RedisValue[]>> tasks = new List<Task<RedisValue[]>>();
                while (clock.ElapsedMilliseconds <= 1000)
                {
                    tasks = new List<Task<RedisValue[]>>();
                    for (var i = 0; i < numeroThreads; i++)
                    {
                        tasks.Add(RedisDataConnection.Instance.StringGetAsync(keys));
                    }
                    await Task.WhenAll(tasks);
                    ops_seg = ops_seg + (inumbstrings * numeroThreads);
                }
                results.Add(numeroThreads, ops_seg);
            }

            Console.WriteLine("End Test...");
            WriteResults(results);
            Console.WriteLine(" \nEnd \n");
        }

        public static async Task TestReadRedisHash()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number Keys: ");
            Console.ResetColor();
            string keys = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Number HashKeys: ");
            Console.ResetColor();
            string hashkeys = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Arrays number threads: ");
            Console.ResetColor();
            string threads = Console.ReadLine();
            
            int nkey = Convert.ToInt32(keys);
            int nhashkeys = Convert.ToInt32(hashkeys);
            List<string> nthreads = threads.Split(',').ToList();

            Dictionary<string, HashSet<string>> ret = new Dictionary<string, HashSet<string>>();
            for (int i = 0; i < nkey; i++)
            {
                ret.Add(i.ToString(), new HashSet<string>());
                for (int j = 0; j < nhashkeys; j++)
                {
                    ret[i.ToString()].Add(j.ToString());
                }
            }

            Dictionary<RedisKey, RedisValue[]> keysRedis = ParseTo.DicRedisValue(ret);
            Stopwatch clock = new Stopwatch();

            Dictionary<int, int> results = new Dictionary<int, int>();
            await RedisDataConnection.Instance.HashGetAsync(keysRedis);

            Console.Write("Run Test...");

            foreach (string numberThreads in nthreads)
            {
                int numeroThreads = Convert.ToInt32(numberThreads);
                int ops_seg = 0;
                clock.Restart();
                List<Task<Dictionary<string, Dictionary<string, string>>>> tasks = new List<Task<Dictionary<string, Dictionary<string, string>>>>();
                while (clock.ElapsedMilliseconds <= 1000)
                {
                    tasks = new List<Task<Dictionary<string, Dictionary<string, string>>>>();
                    for (var i = 0; i < numeroThreads; i++)
                    {
                        tasks.Add(RedisDataConnection.Instance.HashGetTestAsync(keysRedis));
                    }
                    await Task.WhenAll(tasks);
                    ops_seg = ops_seg + (keysRedis.Keys.Count * numeroThreads);
                }
                results.Add(numeroThreads, ops_seg);
            }

            Console.WriteLine("End Test...");
            WriteResults(results);
            Console.WriteLine(" \nEnd \n");
        }

        private static void WriteResults(Dictionary<int, int> results)
        {
            Console.WriteLine("Results: \n");

            StringBuilder sb = new StringBuilder();
            foreach (var kv in results)
            {
                decimal aux = kv.Value / kv.Key;
                decimal per = (aux / results[1]) * 100;
                sb.AppendLine(String.Format("Threads: {0}, ops/s: {1}, % --> {2}", kv.Key, kv.Value, per));
            }
            Console.Write(sb);
        }
    }
}

//using BenchmarkDotNet.Running;
using BenchmarkDotNet.Running;
using System;
using System.Text;

namespace XTG.Benchmark.Redis
{
    public static class Menu
    {
        public static void Start()
        {
            RedisBasic RedisBasic = new RedisBasic();
            string readWrite = "";
            while (readWrite.ToLower() != "0")
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Choose an option (number):");
                sb.AppendLine("1) Read Hash");
                sb.AppendLine("2) Write Hash");
                sb.AppendLine("3) Read String");
                sb.AppendLine("4) Write String");
                sb.AppendLine("5) Test Read String");
                sb.AppendLine("6) Test Read Hash");
                sb.AppendLine("7) Benchmark");
                sb.AppendLine("0) Exit");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(sb.ToString());
                Console.ResetColor();

                readWrite = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                switch (readWrite)
                {
                    case "1":
                        Console.WriteLine();
                        Console.WriteLine("READ OPTION (HASH).");
                        RedisBasic.ReadRedisHash().Wait();
                        break;
                    case "2":
                        Console.WriteLine();
                        Console.WriteLine("WRITE OPTION (HASH).");
                        RedisBasic.WriteRedisHash();
                        break;
                    case "3":
                        Console.WriteLine();
                        Console.WriteLine("READ OPTION (STRING).");
                        RedisBasic.ReadRedisString().Wait();
                        break;
                    case "4":
                        Console.WriteLine();
                        Console.WriteLine("WRITE OPTION (STRING).");
                        RedisBasic.WriteRedisString();
                        break;
                    case "5":
                        Console.WriteLine();
                        Console.WriteLine("TEST READ (STRING).");
                        RedisBasic.TestReadRedisString().Wait();
                        break;
                    case "6":
                        Console.WriteLine();
                        Console.WriteLine("TEST READ (HASH).");
                        RedisBasic.TestReadRedisHash().Wait();
                        break;
                    case "7":
                        Console.WriteLine();
                        Console.WriteLine("BENCHMARK.");
                        var summary = BenchmarkRunner.Run<BenchmarkRedis>();
                        break;
                    case "0":
                        Console.WriteLine("EXIT.");
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("INVALID OPTION.");
                        break;
                }
                //if (readWrite.ToLower() != "3") { Console.ReadLine(); }
            }
        }
    }
}

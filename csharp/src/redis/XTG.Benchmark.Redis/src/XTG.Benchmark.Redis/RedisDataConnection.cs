using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XTG.BarceloHotels.Common.Redis;

namespace XTG.Benchmark.Redis
{
    public static class RedisDataConnection
    {
        //Instancia para acceder a la redis de datos
        private static readonly object SyncLock_LOCK = new object();
        private static RedisBase _mInstance;

        public static RedisBase Instance
        {

            get
            {
                if ((_mInstance == null))
                {

                    lock (SyncLock_LOCK)
                    {
                        //Doble comprobación para no hacerlo si 2 hilos han entrado dentro del primer if
                        if ((_mInstance == null))
                        {
                            //Trace.TraceWarning("Getting RedisAccess connection:[" + CacheHandlerCloudConfiguration.getKeyString("RedisCache.ConnectionString") + "]");
                            _mInstance = Load();
                        }
                    }
                }
                // return the initialized instance of the Singleton Class
                return _mInstance;
            }
        }

        private static RedisBase Load()
        {
            RedisBase redisManager = null;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("\n \n You want to get the configuration from file appsettings.json? y/n:");
            string yn = Console.ReadLine();

            switch (yn.ToLower())
            {
                case "y":
                    redisManager = LoadFromFile();
                    break;
                case "n":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("\n   redis url: ");
                    Console.ResetColor();
                    string url = Console.ReadLine().Trim();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("   redis port: ");
                    Console.ResetColor();
                    int port = Convert.ToInt32(Console.ReadLine().Trim());
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("   redis pass: ");
                    Console.ResetColor();
                    string pass = Console.ReadLine().Trim();
                    redisManager = new RedisBase(url, port, pass);
                    break;
            }

            if (redisManager == null)
            {
                redisManager = LoadFromFile();
            }

            return redisManager;
        }

        private static RedisBase LoadFromFile()
        {
            var configFile = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: false)
                       .Build();

            RedisBase redisManager = new RedisBase(
               configFile["ConnectionString:redisConnection:redisURL"],
               Convert.ToInt32(configFile["ConnectionString:redisConnection:redisPort"]),
               configFile["ConnectionString:redisConnection:redisPassword"]);

            return redisManager;
        }
    }
}

//using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace XTG.Benchmark.Redis
{
    public class BenchmarkRedis
    {
        [Benchmark]
        public async Task ReadHashBenchMark()
        {
            int nkey = 1;
            int nhashkeys = 1;
            int niterations = 1;
            Dictionary<string, HashSet<string>> ret = new Dictionary<string, HashSet<string>>();
            for (int i = 0; i < nkey; i++)
            {
                ret.Add(i.ToString(), new HashSet<string>());
                for (int j = 0; j < nhashkeys; j++)
                {
                    ret[i.ToString()].Add(j.ToString());
                }
            }
            for (int i = 0; i < niterations; i++)
            {
                await RedisDataConnection.Instance.HashGetAsync(ret);
            }
        }

        [Benchmark]
        public async Task ReadStringBenchMark()
        {
            int nkey = 1;
            int niterations = 1;

            HashSet<string> ret = new HashSet<string>();
            for (int i = 0; i < nkey; i++)
            {
                ret.Add(i.ToString());
            }
            for (int i = 0; i < niterations; i++)
            {
                await RedisDataConnection.Instance.StringGetAsync(ret.ToArray());
            }
        }
    }
}

using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XTG.BarceloHotels.Common.Redis.Parse;

namespace XTG.BarceloHotels.Common.Redis
{
    public class RedisBase : RedisLibrary
    {
        //private RedisLibrary _redis { get; }
        public RedisBase(string urlRedis, int portRedis, string passwordRedis) : base(urlRedis, portRedis, passwordRedis)
        {
            //_redis = new RedisLibrary(urlRedis, portRedis, passwordRedis);
        }

        #region String

        public async Task<Dictionary<string, string>> StringGetAsync(string[] keys)
        {
            return ParseTo.Dictionary(keys, await base.StringGetAsync(ParseTo.RedisKey(keys).ToArray()));
        }

        public async Task<RedisValue[]> StringGetAsync(RedisKey[] keys)
        {
            return await base.StringGetAsync(keys);
        }
        
        public async Task<bool> StringSetAsync(Dictionary<string, string> values)
        {
            if (values == null || values.Count == 0) { return true; }
            return await base.StringSetAsync(ParseTo.KeyValuePair(values));
        }
        
        #endregion
        

        #region Hash

        public async Task<bool[]> HashSetAsync(Dictionary<string, Dictionary<string, string>> values)
        {
            return await base.HashSetAsync(ParseTo.DicHashEntry(values));
        }
        
        public async Task<Dictionary<string, Dictionary<string, string>>> HashGetAsync(Dictionary<string, HashSet<string>> keys)
        {
            return await base.HashGetAsync(ParseTo.DicRedisValue(keys));
        }

        public async Task<Dictionary<string, Dictionary<string, string>>> HashGetTestAsync(Dictionary<RedisKey, RedisValue[]> keys)
        {
            return await base.HashGetAsync(keys);
        }

        #endregion
    }
}

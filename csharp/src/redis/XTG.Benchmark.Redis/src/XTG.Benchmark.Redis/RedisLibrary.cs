using StackExchange.Redis;
using System.Threading.Tasks;
using XTG.BarceloHotels.Common.Redis.Parse;
using System.Linq;
using System.Collections.Generic;
using System;

namespace XTG.BarceloHotels.Common.Redis
{
    public class RedisLibrary
    {
        private IDatabaseAsync redisDB;

        public RedisLibrary(string urlRedis, int portRedis, string passwordRedis)
        {
            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints =
                {
                    { urlRedis, portRedis }
                },
                Password = passwordRedis
            };

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);
            redisDB = redis.GetDatabase();
        }

        #region String
      
        internal async Task<RedisValue[]> StringGetAsync(RedisKey[] keys)
        {
            return await redisDB.StringGetAsync(keys);
        }
        
        internal async Task<bool> StringSetAsync(KeyValuePair<RedisKey, RedisValue>[] KeyValue)
        {
            return await redisDB.StringSetAsync(KeyValue);
        }
        
        #endregion

        #region Key

        public async Task<bool> KeyExpiresAsync(RedisKey key, TimeSpan ttl)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return await redisDB.KeyExpireAsync(key, ttl);
            }
            return false;
        }

        #endregion

        #region Hash
        
        public async Task<bool> HashSetAsync(RedisKey redisKey, HashEntry[] hashFields)
        {
            try
            {
                await redisDB.HashSetAsync(redisKey, hashFields);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public async Task<bool[]> HashSetAsync(Dictionary<string, HashEntry[]> values)
        {
            List<Task<bool>> results = new List<Task<bool>>();
            foreach (var value in values)
            {
                results.Add(HashSetAsync(value.Key, value.Value));
            }
            return await Task.WhenAll(results);
        }
        
        public async Task<RedisValue[]> HashGetAsync(RedisKey key, RedisValue[] hashKeys)
        {
            return await redisDB.HashGetAsync(key, hashKeys);
        }
        
        public async Task<Dictionary<string, Dictionary<string, string>>> HashGetAsync(Dictionary<RedisKey, RedisValue[]> keys)
        {
            List<Task<RedisValue[]>> result = new List<Task<RedisValue[]>>();
            foreach (RedisKey key in keys.Keys)
            {
                result.Add(this.HashGetAsync(key, keys[key]));
            }
            var redisValuesArray = await Task.WhenAll(result);
            var ret = new Dictionary<string, Dictionary<string, string>>();
            //int i = 0;
            //foreach (var key in keys.Keys)
            //{
            //    ret.Add(key, new Dictionary<string, string>());
            //    for (int j = 0; j < redisValuesArray[i].Count(); j++)
            //    {
            //        if (!redisValuesArray[i][j].IsNullOrEmpty)
            //        {
            //            ret[key].Add(keys[key][j], redisValuesArray[i][j]);
            //        }
            //    }
            //    i++;
            //}

            return ret;
        }
        
        #endregion
    
    }
}

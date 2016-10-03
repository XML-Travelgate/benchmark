using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace XTG.BarceloHotels.Common.Redis.Parse
{
    public static class ParseTo
    {
        #region Convert to Redis

        internal static RedisKey RedisKey(string key)
        {
            return (RedisKey)key;
        }

        internal static IEnumerable<RedisKey> RedisKey(string[] keys)
        {
            return keys.Select(x => (RedisKey)x);
        }

        internal static RedisValue RedisValue(string value)
        {
            return (RedisValue)value;
        }

        internal static IEnumerable<RedisValue> RedisValue(string[] keys)
        {
            return keys.Select(x => (RedisValue)x);
        }

        internal static IEnumerable<RedisValue> RedisValue(List<string> keys)
        {
            return keys.Select(x => (RedisValue)x);
        }

        internal static IEnumerable<RedisValue> RedisValue(HashSet<string> keys)
        {
            return keys.Select(x => (RedisValue)x);
        }

        internal static KeyValuePair<RedisKey, RedisValue>[] KeyValuePair(Dictionary<string, string> values)
        {
            var newKeys = new KeyValuePair<RedisKey, RedisValue>[values.Count];
            int count = 0;
            foreach (string key in values.Keys)
            {
                newKeys[count] = new KeyValuePair<RedisKey, RedisValue>(key, values[key]);
                count++;
            }
            return newKeys;
        }

        internal static HashEntry[] HashEntry(Dictionary<string, string> hashFields)
        {
            return hashFields.Select(x => new HashEntry(RedisValue(x.Key), RedisValue(x.Value))).ToArray();
        }        

        #endregion
        
        internal static Dictionary<string, string> Dictionary(string[] keys, RedisValue[] redisValue)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            for (int i = 0; i < keys.Length; i++)
            {
                if (!redisValue[i].IsNull) { ret.Add(keys[i], redisValue[i]); }
            }
            return ret;
        }

        internal static Dictionary<string, string> Dictionary(HashEntry[] redisValue)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            foreach (HashEntry value in redisValue)
            {
                ret.Add(value.Name, value.Value);
            }
            return ret;
        }

        internal static Dictionary<string, HashEntry[]> DicHashEntry(Dictionary<string, Dictionary<string, string>> values)
        {
            Dictionary<string, HashEntry[]> ret = new Dictionary<string, StackExchange.Redis.HashEntry[]>();
            foreach (var value in values)
            {
                ret.Add(value.Key, HashEntry(value.Value));
            }
            return ret;
        }

        internal static Dictionary<RedisKey, RedisValue[]> DicRedisValue(Dictionary<string, List<string>> values)
        {
            var ret = new Dictionary<RedisKey, RedisValue[]>();
            foreach (var value in values)
            {
                ret.Add(value.Key, RedisValue(value.Value).ToArray());
            }
            return ret;
        }

        internal static Dictionary<RedisKey, RedisValue[]> DicRedisValue(Dictionary<string, HashSet<string>> values)
        {
            var ret = new Dictionary<RedisKey, RedisValue[]>();
            foreach (var value in values)
            {
                ret.Add(value.Key, RedisValue(value.Value).ToArray());
            }
            return ret;
        }

        internal static string[] Array(RedisValue[] hashKeys)
        {
            return hashKeys.Select(x => (string)x).ToArray();
        }

        

    }
}


using StackExchange.Redis;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _redis;
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }
        public bool Remove(string key)
        {
            return _database.KeyDelete(key);
        }

        public bool Cache(string key, string token, TimeSpan tokenExpiry)
        {
            return _database.StringSet(key, token, tokenExpiry);
        }

        public string? Get(string key)
        {
            return _database.StringGet(key);
        }

        public IEnumerable<string> GetKeys(string pattern)
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            return server.Keys(pattern: pattern).Select(k => k.ToString());
        }

        public void DeleteKeysByPattern(string pattern)
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: pattern).ToArray();
            if (keys.Any())
            {
                _database.KeyDelete(keys);
            }
        }
    }
}

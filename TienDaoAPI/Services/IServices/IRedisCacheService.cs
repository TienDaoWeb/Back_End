namespace TienDaoAPI.Services.IServices
{
    public interface IRedisCacheService
    {
        bool Cache(string key, string value, TimeSpan expiry);
        bool Remove(string key);
        string? Get(string key);
        IEnumerable<string> GetKeys(string pattern);
        void DeleteKeysByPattern(string pattern);
    }
}

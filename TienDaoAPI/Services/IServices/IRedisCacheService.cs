namespace TienDaoAPI.Services.IServices
{
    public interface IRedisCacheService
    {
        bool Cache(string key, string token, TimeSpan tokenExpiry);
        bool Remove(string key);
        string? Get(string key);
        IEnumerable<string> GetKeys(string pattern);
        void DeleteKeysByPattern(string pattern);
    }
}

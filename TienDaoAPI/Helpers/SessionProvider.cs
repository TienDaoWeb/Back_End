using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using TienDaoAPI.DTOs;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Helpers
{
    public class SessionProvider
    {
        private const string PrefixKey = "session";
        private readonly IRedisCacheService _redisCacheService;

        public SessionProvider(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        public bool VerifyToken(string token)
        {
            try
            {
                var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userId = accessToken.Payload["sub"]?.ToString();
                var jti = accessToken.Payload["jti"]?.ToString();
                var result = _redisCacheService.Get($"{PrefixKey}:{userId}:access:{jti}");
                return result != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public bool VerifyRefreshToken(string token)
        {
            try
            {
                var refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userId = refreshToken.Payload["sub"]?.ToString();
                var jti = refreshToken.Payload["jti"]?.ToString();

                var result = _redisCacheService.Get($"{PrefixKey}:{userId}:refresh:{jti}");
                return result != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public UserDTO? GetContext(string token)
        {
            try
            {
                var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userId = accessToken.Payload["sub"]?.ToString();
                var jti = accessToken.Payload["jti"]?.ToString();

                var key = $"{PrefixKey}:{userId}:access:{jti}";
                var sessionData = _redisCacheService.Get(key);

                return JsonConvert.DeserializeObject<UserDTO>(sessionData!);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public void SaveSession(string key, UserDTO data, string accessJti, string refreshJti)
        {
            RemoveSession(key, "access");
            RemoveSession(key, "refresh");

            var dataJson = JsonConvert.SerializeObject(data);
            _redisCacheService.Cache($"{PrefixKey}:{key}:access:{accessJti}", dataJson, TimeSpan.FromHours(3));
            _redisCacheService.Cache($"{PrefixKey}:{key}:refresh:{refreshJti}", dataJson, TimeSpan.FromDays(50));
        }

        private void RemoveSession(string key, string type)
        {
            var pattern = $"{PrefixKey}:{key}:{type}:*";
            _redisCacheService.DeleteKeysByPattern(pattern);
        }
    }
}

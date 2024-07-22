namespace TienDaoAPI.Helpers
{
    public class CustomTwoFactorTokenProviderOptions
    {
        public TimeSpan TokenLifespan { get; set; } = TimeSpan.FromMinutes(15);
    }
}

using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;
using TienDaoAPI.Data;
using TienDaoAPI.Helpers;
using TienDaoAPI.Middlewares;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services;
using TienDaoAPI.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TienDaoDbContext>(option =>
{
    //option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    option.UseNpgsql(builder.Configuration.GetConnectionString("PostpreConnection"));
});

builder.Services.AddIdentityCore<User>()
    .AddTokenProvider<CustomTwoFactorTokenProvider>("CustomTwoFactorTokenProvider")
    .AddEntityFrameworkStores<TienDaoDbContext>();

var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    options.Tokens.EmailConfirmationTokenProvider = "CustomTwoFactorTokenProvider";

});

builder.Services.Configure<CustomTwoFactorTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(15);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddOptions();
var mailsettings = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsettings);

builder.Services.AddTransient<EmailProvider>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "jwtToken_Auth_API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
            },
            new string[] {}
        }
    });
});

//Service firebase storage
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"Services/FileState/certificate-tiendaoapi.json");
builder.Services.AddSingleton<IFirebaseStorageService>(s => new FirebaseStorageService(StorageClient.Create()));

builder.Services.AddScoped<IRedisCacheService>(provider =>
{
    return new RedisCacheService(provider.GetRequiredService<IConnectionMultiplexer>());
});
//builder.Services.AddSingleton<ITokenStoreService, RedisCacheService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<JwtHandler>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

//builder.Services.AddControllersWithViews(options =>
//{
//    options.Filters.Add<CustomAuthorizeAttribute>();
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();
app.UseHttpsRedirection();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

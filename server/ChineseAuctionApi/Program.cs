using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories;
using ChineseAuctionApi.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using ChineseAuctionApi.Services.aa;
using ChineseAuctionApi.Services.AuthUser;
using ChineseAuctionApi.Services.Cards;
using ChineseAuctionApi.Services.Donors;
using ChineseAuctionApi.Services.Gifts;
using ChineseAuctionApi.Services.Lotteries;
using ChineseAuctionApi.Repositories.AuthUsers;
using ChineseAuctionApi.Repositories.Baskets;
using ChineseAuctionApi.Repositories.Cards;
using ChineseAuctionApi.Repositories.Donors;
using ChineseAuctionApi.Repositories.Gifts;
using ChineseAuctionApi.Repositories.Lotteries;



var builder = WebApplication.CreateBuilder(args);



builder.Services.AddScoped<IGiftRepository, GiftRepository>();
builder.Services.AddScoped<IDonorRepository, DonorRepository>();
builder.Services.AddScoped<IGiftService, GiftService>();
builder.Services.AddScoped<IDonorService, DonorService>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IAuthUserRepositoty, AuthUserRepository>();
builder.Services.AddScoped<IAuthUserService, AuthUserService>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<ILotteryRepository, LotteryRepository>();
builder.Services.AddScoped<ILotteryService, LotteryService > ();

builder.Services.AddScoped<JwtTokenService>();

builder.Services.AddControllers();


builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});


builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ChineseAuctionApi",
        Description = "A simple example ASP.NET Core API to manage ChineseAuction",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your.email@example.com",
            Url = new Uri("https://yourwebsite.com"),
        }
    });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer {your_token}'"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "origins",
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader().
                              AllowAnyMethod();
                          });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gifts API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseRouting();

app.UseAuthentication(); 

app.UseAuthorization(); 

app.UseCors("origins");

app.UseHttpsRedirection();



app.MapControllers();

app.Run();

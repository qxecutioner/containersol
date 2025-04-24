using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Polly;
using System.Text;
using WebApplication4.Extensions;
using WebApplication4.Models;
using WebApplication4;
using WebApplication4.Models;
using WebApplication4.Services;
using WebApplication4.Services.Interfaces;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOption<AppSettings>();
builder.Services.AddOption<AzureAd>();

builder.Services.AddHttpClient<IApiServices<List<TransferData>>, ApiServices<List<TransferData>>>(client =>
{
    
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.RetryAsync(3))
.AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

builder.Services.AddAuthentication(x => {

    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["AzureAd:Issuer"],
            ValidAudience = config["AzureAd:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AzureAd:IssuerKey"]))
        };
    });

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "dev-redis-service:6379,password=test123,defaultDatabase=0";
    
});

builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(20));

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.Run();

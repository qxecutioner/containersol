using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => { 



});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

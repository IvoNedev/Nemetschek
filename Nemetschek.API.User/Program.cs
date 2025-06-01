using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nemetschek.API.User.Extensions;
using Nemetschek.Data.Contexts;
using Nemetschek.Data.Models.usr;
using Nemetschek.Services;
using Nemetschek.Services.Constants;
using Nemetschek.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Add the DI types (services and repos)
builder.Services.AddDataServices();

//Add the dbContext
builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Add swagger
builder.Services.AddCustomSwagger();

//Add JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddRateLimitingPolicy();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();
app.Run();
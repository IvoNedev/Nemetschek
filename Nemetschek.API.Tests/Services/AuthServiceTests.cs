using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Nemetschek.Data.Contexts;
using Nemetschek.Data.Models.usr;
using Nemetschek.Services;
using Nemetschek.Services.Interfaces;
using Nemetschek.API.Contract.Auth.Request;
using Nemetschek.API.Contract.Auth.Response;
using Microsoft.Extensions.Options;

namespace Nemetschek.API.Tests.Services
{
    public class AuthServiceTests
    {
        private (IAuthService, AppDbContext) BuildAuthServiceWithInMemoryDb(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var ctx = new AppDbContext(options);

            var hasher = new PasswordHasher<User>();
            var repo = new Nemetschek.Data.Repositories.NemetschekRepo(ctx);

            // Seed one user:
            var user = new User
            {
                FirstName = "Carol",
                LastName = "White",
                Email = "carol@example.com"
            };
            user.PasswordHash = hasher.HashPassword(user, "Test123!");
            ctx.User.Add(user);
            ctx.SaveChanges();

            // Fake JWT settings
            var jwtSettings = new Nemetschek.Services.Constants.JwtSettings
            {
                Key = "ThisIsAVerySecureTestKey12345",
                Issuer = "TestIssuer",
                Audience = "TestAudience"
            };
            var optionsMock = Options.Create(jwtSettings);

            var svc = new AuthService(repo, hasher, optionsMock);
            return (svc, ctx);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsToken_OnValidCredentials()
        {
            var (svc, ctx) = BuildAuthServiceWithInMemoryDb("AuthDb1");
            var req = new AuthRequest
            {
                Email = "carol@example.com",
                Password = "Test123!"
            };

            var resp = await svc.AuthenticateAsync(req);
            Assert.NotNull(resp);
            Assert.False(string.IsNullOrEmpty(resp.Token));

            // Optionally: validate that “sub” claim is a GUID that matches “user.Id”
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsNull_OnWrongPassword()
        {
            var (svc, ctx) = BuildAuthServiceWithInMemoryDb("AuthDb2");
            var req = new AuthRequest
            {
                Email = "carol@example.com",
                Password = "WrongPassword"
            };

            var resp = await svc.AuthenticateAsync(req);
            Assert.Null(resp);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsNull_OnUnknownEmail()
        {
            var (svc, ctx) = BuildAuthServiceWithInMemoryDb("AuthDb3");
            var req = new AuthRequest
            {
                Email = "nobody@nowhere.com",
                Password = "irrelevant"
            };

            var resp = await svc.AuthenticateAsync(req);
            Assert.Null(resp);
        }
    }
}

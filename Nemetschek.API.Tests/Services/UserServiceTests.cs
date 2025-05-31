using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Nemetschek.Data.Contexts;
using Nemetschek.Data.Models.usr;
using Nemetschek.Services;
using Nemetschek.Services.Interfaces;
using Nemetschek.API.Contract.User.Request;
using Nemetschek.API.Contract.User.Response;

namespace Nemetschek.API.Tests.Services
{
    public class UserServiceTests
    {
        private IUserService BuildUserServiceWithInMemoryDb(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var ctx = new AppDbContext(options);

            var hasher = new PasswordHasher<User>();
            // We can use a real hasher here.

            // We’ll need a real repo implementation, so:
            var repo = new Nemetschek.Data.Repositories.NemetschekRepo(ctx);

            return new UserService(repo, hasher);
        }

        [Fact]
        public async Task CreateAsync_StoresUserAndReturnsResponse()
        {
            var svc = BuildUserServiceWithInMemoryDb("TestDb1");

            var fakeImage = new FormFile(
                baseStream: new MemoryStream(new byte[] { 1, 2, 3 }),
                baseStreamOffset: 0,
                length: 3,
                name: "image",
                fileName: "photo.jpg");

            var req = new CreateUserRequest
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com",
                Password = "P@ssw0rd!",
                Image = fakeImage
            };

            var resp = await svc.CreateAsync(req);

            Assert.NotNull(resp);
            Assert.Equal("Alice", resp.FirstName);
            Assert.Equal("Smith", resp.LastName);
            Assert.Equal("alice@example.com", resp.Email);
            Assert.NotNull(resp.PhotoBase64);
            Assert.True(resp.PhotoBase64.Length > 0);

            // Also verify it’s actually in the database with a hashed password:
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb1")
                .Options;
            using var ctx2 = new AppDbContext(options);
            var stored = await ctx2.User.SingleOrDefaultAsync(u => u.Email == "alice@example.com");
            Assert.NotNull(stored);
            Assert.NotEqual("P@ssw0rd!", stored.PasswordHash); // must be hashed
        }

        [Fact]
        public async Task CreateAsync_ThrowsIfEmailMissing()
        {
            var svc = BuildUserServiceWithInMemoryDb("TestDb2");

            var req = new CreateUserRequest
            {
                FirstName = "Bob",
                LastName = "Brown",
                Email = "",        // invalid
                Password = "",     // invalid
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => svc.CreateAsync(req));
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Nemetschek.Data.Contexts;
using Nemetschek.Data.Models.usr;
using Nemetschek.Data.Models.dice;
using Nemetschek.Data.Repositories;

namespace Nemetschek.Data.Tests.Repositories
{
    public class NemetschekRepoTests
    {
        private (NemetschekRepo Repo, AppDbContext Context) BuildRepo(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var ctx = new AppDbContext(options);
            return (new NemetschekRepo(ctx), ctx);
        }

        [Fact(DisplayName = "AddUserAsync and GetUserByEmailAsync work correctly")]
        public async Task AddUser_And_GetUserByEmail_Works()
        {
            var (repo, ctx) = BuildRepo("RepoTestDb_Users");

            var user = new User
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com"
            };

            await repo.AddUserAsync(user);
            await repo.SaveChangesAsync();

            var fetched = await repo.GetUserByEmailAsync("alice@example.com");
            Assert.NotNull(fetched);
            Assert.Equal("Alice", fetched!.FirstName);
            Assert.Equal("Smith", fetched.LastName);
            Assert.Equal("alice@example.com", fetched.Email);
        }

        [Fact(DisplayName = "GetUserByEmailAsync returns null when not found")]
        public async Task GetUserByEmail_ReturnsNull_IfNotExists()
        {
            var (repo, ctx) = BuildRepo("RepoTestDb_NoUser");

            var fetched = await repo.GetUserByEmailAsync("doesnotexist@example.com");
            Assert.Null(fetched);
        }

        [Fact(DisplayName = "AddDiceRollAsync and GetDiceRollsByUser work correctly")]
        public async Task AddDiceRoll_And_GetDiceRollsByUser_Works()
        {
            var (repo, ctx) = BuildRepo("RepoTestDb_Dice");

            var userId = Guid.NewGuid();
            var roll1 = new DiceRoll
            {
                UserId = userId,
                Die1 = 2,
                Die2 = 5,
                RolledAt = new DateTime(2023, 01, 01)
            };
            var roll2 = new DiceRoll
            {
                UserId = userId,
                Die1 = 3,
                Die2 = 4,
                RolledAt = new DateTime(2023, 01, 02)
            };

            await repo.AddDiceRollAsync(roll1);
            await repo.AddDiceRollAsync(roll2);
            await repo.SaveChangesAsync();

            var rolls = repo.GetDiceRollsByUser(userId).ToList();
            Assert.Equal(2, rolls.Count);

            Assert.Contains(rolls, r => r.Die1 == 2 && r.Die2 == 5);
            Assert.Contains(rolls, r => r.Die1 == 3 && r.Die2 == 4);
        }

        [Fact(DisplayName = "GetDiceRollsByUser returns empty when no rolls exist")]
        public void GetDiceRollsByUser_ReturnsEmpty_IfNone()
        {
            var (repo, ctx) = BuildRepo("RepoTestDb_DiceEmpty");
            var rolls = repo.GetDiceRollsByUser(Guid.NewGuid()).ToList();
            Assert.Empty(rolls);
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Nemetschek.Data.Contexts;
using Nemetschek.Data.Models.dice;
using Nemetschek.Services;
using Nemetschek.Services.Interfaces;
using Nemetschek.API.Contract.Dice.Request;
using Nemetschek.API.Contract.Dice.Response;
using Nemetschek.API.Contract.Dice.Request;

namespace Nemetschek.API.Tests.Services
{
    public class DiceServiceTests
    {
        private IDiceService BuildDiceServiceWithInMemoryDb(string dbName, Guid userId, out AppDbContext ctx)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            ctx = new AppDbContext(options);

            var repo = new Nemetschek.Data.Repositories.NemetschekRepo(ctx);
            var svc = new DiceService(repo);

            ctx.DiceRoll.AddRange(
                new DiceRoll { UserId = userId, Die1 = 1, Die2 = 1, RolledAt = new DateTime(2023, 01, 01) },
                new DiceRoll { UserId = userId, Die1 = 2, Die2 = 2, RolledAt = new DateTime(2023, 02, 15) },
                new DiceRoll { UserId = userId, Die1 = 3, Die2 = 3, RolledAt = new DateTime(2023, 04, 10) },
                new DiceRoll { UserId = userId, Die1 = 4, Die2 = 4, RolledAt = new DateTime(2023, 04, 20) },
                new DiceRoll { UserId = userId, Die1 = 5, Die2 = 5, RolledAt = new DateTime(2023, 05, 05) }
            );
            ctx.SaveChanges();
            return svc;
        }

        [Fact]
        public async Task RollAsync_CreatesNewDiceRoll()
        {
            var userId = Guid.NewGuid();
            var svc = BuildDiceServiceWithInMemoryDb("DiceDb1", userId, out var ctx);

            var resp = await svc.RollAsync(userId);

            Assert.NotNull(resp);
            Assert.InRange(resp.Die1, 1, 6);
            Assert.InRange(resp.Die2, 1, 6);


            var allRolls = ctx.DiceRoll.Where(r => r.UserId == userId).ToList();
            Assert.Equal(6, allRolls.Count); 
        }

        [Fact]
        public async Task PopulateTestDataForFiltering_Creates8Rolls()
        {
            var userId = Guid.NewGuid();
            var svc = BuildDiceServiceWithInMemoryDb("DiceDb3", userId, out var ctx);

            await svc.PopulateTestDataForFilteringAsync(userId);

            var all = ctx.DiceRoll.Where(r => r.UserId == userId).ToList();
            Assert.Equal(13, all.Count);
        }
    }
}

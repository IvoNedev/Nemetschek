using Microsoft.EntityFrameworkCore;
using Nemetschek.API.Contract.Dice.Request;
using Nemetschek.API.Contract.Dice.Response;
using Nemetschek.Data.Contexts;
using Nemetschek.Data.Models.dice;
using Nemetschek.Data.Repositories.Interfaces;
using Nemetschek.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Services
{
    public class DiceService : IDiceService
    {
        private readonly INemetschekRepo _repo;

        public DiceService(INemetschekRepo repo)
        {
            _repo = repo;
        }

        ///<inheritdoc/>
        public async Task<DiceRollResponse> RollAsync(Guid userId)
        {
            //Roll the dice
            var rnd = new Random();
            var roll = new DiceRoll
            {
                UserId = userId,
                Die1 = rnd.Next(1, 7),
                Die2 = rnd.Next(1, 7)
            };

            await _repo.AddDiceRollAsync(roll);
            await _repo.SaveChangesAsync();

            //Tell the user what happened
            return new DiceRollResponse
            {
                Id = roll.Id,
                Die1 = roll.Die1,
                Die2 = roll.Die2,
                RolledAt = roll.RolledAt
            };
        }

        ///<inheritdoc/>
        public async Task<GetDiceRollsResponse> GetAsync(Guid userId, GetDiceHistoryRequest request)
        {
            // Get the User's rolls
            var allUserRolls = _repo.GetDiceRollsByUser(userId);

            // Apply filters
            if (request.Year.HasValue)
                allUserRolls = allUserRolls.Where(r => r.RolledAt.Year == request.Year.Value);

            if (request.Month.HasValue)
                allUserRolls = allUserRolls.Where(r => r.RolledAt.Month == request.Month.Value);

            if (request.Day.HasValue)
                allUserRolls = allUserRolls.Where(r => r.RolledAt.Day == request.Day.Value);

            // Determine if ascending or descending
            bool asc = string.Equals(request.SortDir, "asc", StringComparison.OrdinalIgnoreCase);

            // Check if client wants “sum” as the primary key
            bool sortBySum = request.SortBy?.Equals("sum", StringComparison.OrdinalIgnoreCase) == true;

            if (sortBySum)
            {
                if (asc)
                    allUserRolls = allUserRolls.OrderBy(r => r.Die1 + r.Die2)
                         .ThenBy(r => r.RolledAt);
                else
                    allUserRolls  = allUserRolls.OrderByDescending(r => r.Die1 + r.Die2)
                         .ThenByDescending(r => r.RolledAt);
            }
            else //Then it's {sort by 'datetime'}
            {
                if (asc)
                    allUserRolls = allUserRolls.OrderBy(r => r.RolledAt)
                         .ThenBy(r => r.Die1 + r.Die2);
                else
                    allUserRolls = allUserRolls.OrderByDescending(r => r.RolledAt)
                         .ThenByDescending(r => r.Die1 + r.Die2);
            }

            //Count total before pagination
            var total = await allUserRolls.CountAsync();

            //Apply pagination
            var subset = allUserRolls
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            // Convert to a response object
            var items = await subset
                .Select(r => new DiceRollResponse
                {
                    Id = r.Id,
                    Die1 = Convert.ToInt32(r.Die1),
                    Die2 = Convert.ToInt32(r.Die2),
                    RolledAt = r.RolledAt
                })
                .ToListAsync();

            //Tell the user the results
            return new GetDiceRollsResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = total
            };
        }

        public async Task PopulateTestDataForFilteringAsync(Guid userId)
        {
            var testRolls = new[]
            {
                // Two entries in different years (2025, 2024)
                new DiceRoll { UserId = userId, Die1 = 3, Die2 = 4, RolledAt = new DateTime(2025, 01, 01) },
                new DiceRoll { UserId = userId, Die1 = 5, Die2 = 2, RolledAt = new DateTime(2024, 02, 02) },

                // Two entries in same year (2023) and same month (April)
                new DiceRoll { UserId = userId, Die1 = 1, Die2 = 6, RolledAt = new DateTime(2023, 04, 01) },
                new DiceRoll { UserId = userId, Die1 = 2, Die2 = 5, RolledAt = new DateTime(2023, 04, 15) },

                // Two entries in same year (2023) but different month (May)
                new DiceRoll { UserId = userId, Die1 = 4, Die2 = 4, RolledAt = new DateTime(2023, 05, 01) },
                new DiceRoll { UserId = userId, Die1 = 6, Die2 = 1, RolledAt = new DateTime(2023, 05, 15) },

                // Two entries on the exact same date (2022-06-10)
                new DiceRoll { UserId = userId, Die1 = 3, Die2 = 3, RolledAt = new DateTime(2022, 06, 10) },
                new DiceRoll { UserId = userId, Die1 = 5, Die2 = 5, RolledAt = new DateTime(2022, 06, 10) }
            };

            foreach (var roll in testRolls)
            {
                await _repo.AddDiceRollAsync(roll);
            }
            await _repo.SaveChangesAsync();
        }
    }
}

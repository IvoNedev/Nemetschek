using Microsoft.EntityFrameworkCore;
using Nemetschek.Data.Contexts;
using Nemetschek.Data.Models.dice;
using Nemetschek.Data.Models.usr;
using Nemetschek.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Data.Repositories
{
    public class NemetschekRepo : INemetschekRepo
    {
        private readonly AppDbContext _context;
        public NemetschekRepo(AppDbContext context)
        {
            _context = context;
        }

        ///<inheritdoc/>
        public async Task AddUserAsync(User user)
        {
            _context.User.Add(user);
            await Task.CompletedTask;
        }

        ///<inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        ///<inheritdoc/>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.User.SingleOrDefaultAsync(u => u.Email == email);
        }

        ///<inheritdoc/>
        public async Task AddDiceRollAsync(DiceRoll roll)
        {
            _context.DiceRoll.Add(roll);
            await Task.CompletedTask;
        }

        ///<inheritdoc/>
        public IQueryable<DiceRoll> GetDiceRollsByUser(Guid userId)
        {
            return _context.DiceRoll.AsNoTracking().Where(r => r.UserId == userId);
        }
    }
}

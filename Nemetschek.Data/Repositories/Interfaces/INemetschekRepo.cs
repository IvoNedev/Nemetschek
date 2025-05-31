using Nemetschek.Data.Models.dice;
using Nemetschek.Data.Models.usr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Data.Repositories.Interfaces
{
    public interface INemetschekRepo
    {
        //General SaveChanges func
        Task SaveChangesAsync();

        /// <summary>
        /// FUnction to create a new user
        /// </summary>
        Task AddUserAsync(User user);

        /// <summary>
        /// Function to attempt to obtain a user by a given email
        /// </summary>
        /// <param name="email">the email string</param>
        /// <returns><see cref="User"/></returns>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Function to add a 2x dice roll to the DB
        /// </summary>
        /// <param name="roll"><see cref="DiceRoll"/></param>
        Task AddDiceRollAsync(DiceRoll roll);

        /// <summary>
        /// Function to obtain a given user's dice roll history
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="DiceRoll"/></returns>
        IQueryable<DiceRoll> GetDiceRollsByUser(Guid userId);

    }
}

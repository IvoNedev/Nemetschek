using Nemetschek.API.Contract.Dice.Request;
using Nemetschek.API.Contract.Dice.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Services.Interfaces
{
    public interface IDiceService
    {
        /// <summary>
        /// Function that simulates a 2x dice roll
        /// </summary>
        /// <param name="userId">The user Id that's rolling the dice</param>
        /// <returns><see cref="DiceRollResponse"/></returns>
        Task<DiceRollResponse> RollAsync(Guid userId);

        /// <summary>
        /// Method that returns the dice roll history and enables filtering
        /// </summary>
        /// <param name="userId">The user Id that's rolling the dice</param>
        /// <param name="request">Any filtering required by the user</param>
        /// <returns><see cref="GetDiceRollsResponse"/></returns>
        Task<GetDiceRollsResponse> GetAsync(Guid userId, GetDiceHistoryRequest request);

        /// <summary>
        /// Populate db with sample rolls to allow filtering
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task PopulateTestDataForFilteringAsync(Guid userId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.API.Contract.Dice.Response
{
    public class GetDiceRollsResponse
    {
        public IEnumerable<DiceRollResponse> Items { get; init; } = null!;
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
    }
}

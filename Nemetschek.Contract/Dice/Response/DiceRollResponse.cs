using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.API.Contract.Dice.Response
{
    public class DiceRollResponse
    {
        public Guid Id { get; init; }
        public int Die1 { get; init; }
        public int Die2 { get; init; }
        public int Sum => Die1 + Die2;
        public DateTime RolledAt { get; init; }
    }
}

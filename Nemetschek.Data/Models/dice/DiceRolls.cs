 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nemetschek.Data.Constants;

namespace Nemetschek.Data.Models.dice
{
    /// <summary>
    /// Class type that represents the structure of the <see cref="Tables.DiceRolls"/> table
    /// </summary>
    [Table(Tables.DiceRoll, Schema = Schemas.Dice)]
    public class DiceRoll
    {
        /// <summary>
        /// Gets or sets the Primary Key
        /// </summary>
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the UserId linked to a Dice Roll
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The result for roll #1
        /// </summary>
        public int Die1 { get; set; }

        /// <summary>
        /// The result for roll #2
        /// </summary>
        public int Die2 { get; set; }

        /// <summary>
        /// When did the user roll the dice
        /// </summary>
        public DateTime RolledAt { get; set; } = DateTime.UtcNow;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Data.Constants
{
    /// <summary>
    /// Public static class containing constants for database table names (linking to EF entities).
    /// </summary>
    public static class Tables
    {
        /// <summary>
        /// A constant representing the Test table name.
        /// </summary>
        public const string Test = "Test";

        /// <summary>
        /// A constant representing the User table name.
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// A constant representing the DiceRolls table name.
        /// </summary>
        public const string DiceRoll = "DiceRoll";
    }
}

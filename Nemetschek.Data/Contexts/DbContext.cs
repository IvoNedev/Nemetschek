using Microsoft.EntityFrameworkCore;
using Nemetschek.Data.Models.dbo;
using Nemetschek.Data.Models.dice;
using Nemetschek.Data.Models.usr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Data.Contexts
{
    /// <summary>
    /// Represents a database context for interacting with entities from the database.    
    /// </summary>
    /// <seealso cref="DbContext" />
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        ///<inheritdoc/>
        public DbSet<Test> Test { get; set; }
        ///<inheritdoc/>
        public DbSet<User> User { get; set; }
        ///<inheritdoc/>
        public DbSet<DiceRoll> DiceRoll { get; set; }
    }
}

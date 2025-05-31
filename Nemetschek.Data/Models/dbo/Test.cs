using Nemetschek.Data.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Data.Models.dbo
{
    /// <summary>
    /// Class type that represents the structure of the <see cref="Tables.Test"/> table
    /// </summary>
    [Table(Tables.Test, Schema = Schemas.Dbo)]
    public class Test
    {
        /// <summary>
        /// Gets or sets the PrimaryKey
        /// </summary>
        public int TestId { get; set; }
    }
}

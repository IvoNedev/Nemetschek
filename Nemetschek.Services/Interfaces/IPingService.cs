using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Services.Interfaces
{
    /// <summary>
    /// Service to test the DB connection
    /// </summary>
    public interface IPingtService
    {
        /// <summary>
        /// Simple ping (get count of known table)
        /// </summary>
        /// <returns></returns>
        bool Ping();
    }
}

using Nemetschek.Data.Contexts;
using Nemetschek.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Services
{
    public class PingService : IPingtService
    {
        private readonly AppDbContext _context;

        public PingService(AppDbContext context)
        {
            _context = context;
        }

        ///<inheritdoc/>
        public bool Ping()
        {
            try
            {
                bool anyTests = _context.Test.Any();
                return anyTests;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nemetschek.Data.Constants;

namespace Nemetschek.Data.Models.usr
{
    /// <summary>
    /// Class type that represents the structure of the <see cref="Tables.User"/> table
    /// </summary>
    [Table(Tables.User, Schema = Schemas.Usr)]
    public class User
    {
        /// <summary>
        /// Gets or sets the Primary Key
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the User's first name
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the User's last name
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the User's email
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the User's password hash
        /// </summary>
        public string PasswordHash { get; set; } = null!;

        /// <summary>
        /// Gets or sets the User's photo as a base64
        /// </summary>
        public string? PhotoBase64 { get; set; }

        /// <summary>
        /// Gets or sets the User's registration time
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.API.Contract.User.Request
{
    public class CreateUserRequest
    {
        [FromForm(Name = "firstName")]
        [Required]

        public string FirstName { get; set; } = default!;

        [FromForm(Name = "lastName")]
        [Required]
        public string LastName { get; set; } = default!;

        [FromForm(Name = "email")]
        [Required] 
        [EmailAddress]//This would usually be done in the frontend
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string Email { get; set; } = default!;

        [FromForm(Name = "password")]
        [Required] 
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")] //This would usually be done in the frontend
        public string Password { get; set; } = default!;

        [FromForm(Name = "image")]
        public IFormFile? Image { get; set; }
    }
}

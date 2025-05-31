using Microsoft.AspNetCore.Identity;
using Nemetschek.API.Contract.User.Request;
using Nemetschek.API.Contract.User.Response;
using Nemetschek.Data.Contexts;
using Nemetschek.Data.Models.usr;
using Nemetschek.Data.Repositories.Interfaces;
using Nemetschek.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Services
{
    public class UserService : IUserService
    {
        private readonly INemetschekRepo _repo;
        private readonly IPasswordHasher<User> _hasher;

        /// <summary>
        /// The services responsible for the user registration (and in the future any other user related tasks, get, put, delete, etc)
        /// </summary>
        public UserService(INemetschekRepo repo, IPasswordHasher<User> hasher)
        {
            _repo = repo;
            _hasher = hasher;
        }

        ///<inheritdoc/>
        public async Task<CreateUserResponse> CreateAsync(CreateUserRequest request)
        {

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            // 1) Check for duplicate email
            var existing = await _repo.GetUserByEmailAsync(normalizedEmail);
            if (existing != null)
            {
                // Throw a validation exception; our global handler will turn this into 400 Bad Request
                throw new ValidationException($"Email '{normalizedEmail}' is already registered.");
            }

            var user = new User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim().ToLowerInvariant()
            };

            //Make sure the pwd is well protected
            user.PasswordHash = _hasher.HashPassword(user, request.Password);

            //Storing as base64 purely to enable quicker testing once deployed. Didn't feel like dealing with permissions on the server
            //In a production env the image would be stored in a blol or file store
            if (request.Image?.Length > 0)
            {
                using var ms = new MemoryStream();
                await request.Image.CopyToAsync(ms);
                user.PhotoBase64 = Convert.ToBase64String(ms.ToArray());
            }

            //Save
            await _repo.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            //Tell the user the good news
            return new CreateUserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhotoBase64 = user.PhotoBase64
            };
        }
    }
}

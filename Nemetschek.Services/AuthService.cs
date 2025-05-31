using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nemetschek.API.Contract.Auth.Request;
using Nemetschek.API.Contract.Auth.Response;
using Nemetschek.Data.Contexts;
using Nemetschek.Data.Models.usr;
using Nemetschek.Data.Repositories.Interfaces;
using Nemetschek.Services.Constants;
using Nemetschek.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Services
{
    public class AuthService : IAuthService
    {
        private readonly INemetschekRepo _repo;
        private readonly IPasswordHasher<User> _hasher;
        private readonly JwtSettings _jwt;

        public AuthService(
            INemetschekRepo repo,
            IPasswordHasher<User> hasher,
            IOptions<JwtSettings> jwtOptions)
        {
            _repo = repo;
            _hasher = hasher;
            _jwt = jwtOptions.Value;
        }

        ///<inheritdoc/>
        public async Task<AuthResponse?> AuthenticateAsync(AuthRequest req)
        {
            //Get the email and see if a user with that email exists
            var email = req.Email.Trim().ToLowerInvariant();
            var user = await _repo.GetUserByEmailAsync(email);
            if (user is null) return null;

            //If the user exists check if what they entered now as a PWD matches what they originally sent
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (result == PasswordVerificationResult.Failed) return null;

            //If everything is fine then give the user a token, they've earned it
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: new[] { new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()) },
                expires: expires,
                signingCredentials: creds
            );

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
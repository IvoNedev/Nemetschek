using Nemetschek.API.Contract.Auth.Request;
using Nemetschek.API.Contract.Auth.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Attempts to authenticate a user with a given uname (email) and pwd
        /// </summary>
        /// <param name="request">THe user's email and pwd</param>
        /// <returns><see cref="AuthResponse"/></returns>
        Task<AuthResponse> AuthenticateAsync(AuthRequest request);
    }
}

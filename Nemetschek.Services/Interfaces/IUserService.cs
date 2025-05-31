using Nemetschek.API.Contract.User.Request;
using Nemetschek.API.Contract.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Create a user, provided all necessary fields are correctly filled
        /// </summary>
        /// <param name="request">An <see cref="CreateUserRequest"></see> object with all the data needed to create a new user</param>
        /// <returns>An <see cref="CreateUserResponse"></see> object with the newly created user details as they have been stored in the DB</returns>
        Task<CreateUserResponse> CreateAsync(CreateUserRequest request);
    }
}

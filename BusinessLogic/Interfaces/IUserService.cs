using EAD_APP.Core.Enums;
using EAD_APP.Core.Models;
using EAD_APP.Core.Requests;

namespace EAD_APP.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string id);
        Task<User> LoginUser(LoginRequest request);
        Task<bool> UpdateStatus(User user, ActiveStatus status);
    }
}

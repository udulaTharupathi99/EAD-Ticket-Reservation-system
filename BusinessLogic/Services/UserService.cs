////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ScheduleService.cs
//Author : IT20134358
//Created On : 9/10/2023 
//Description : UserService service 
////////////////////////////////////////////////////////////////////////////////////////////////////////
using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Enums;
using EAD_APP.Core.Models;
using EAD_APP.Core.Requests;
using MongoDB.Driver;
using BCryptNet = BCrypt.Net.BCrypt;

namespace EAD_APP.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(IMongoDatabase mongoDatabase)
        {
            _userCollection = mongoDatabase.GetCollection<User>("user"); 
        }

        //add new user
        public async Task<bool> CreateUser(User request)
        {
            //check NIC
            var userModel =  await _userCollection.Find(u => u.Email == request.Email || u.NIC == request.NIC).FirstOrDefaultAsync();

            if (userModel != null)
            {
                throw new Exception("A user with the same email or NIC already exists.");
            }
            
            request.Password = BCryptNet.HashPassword(request.Password);
            await _userCollection.InsertOneAsync(request);
            return true;
        }
        
        //user login
        public async Task<User> LoginUser(LoginRequest request)
        {
            var user =  await _userCollection.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
            
            if (user != null)
            {
                var res = BCryptNet.Verify(request.Password, user.Password);

                if (!res)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("User not found.");
            }
            
            return user;
        }

        //change user status(active, deactivate)
        public async Task<bool> UpdateStatus(User user, ActiveStatus status)
        {
            user.Status = status;
            var res = await _userCollection.ReplaceOneAsync(x => x.Id == user.Id, user);

            return true;
        }

        //delete user
        public async Task<bool> DeleteUser(string id)
        {
            var res = await _userCollection.DeleteOneAsync(x => x.Id == id);
            return true;
        }

        //get all office users
        public async Task<List<User>> GetAllOfficeUsers()
        {
            var users = await _userCollection.Find(_ => true).ToListAsync();
            var users1 = await _userCollection.Find(u=> u.Role == RoleType.BackOffice || u.Role == RoleType.TravelAgent).ToListAsync();
            
            return users1;
        }
        
        //get all traveler users
        public async Task<List<User>> GetAllTravelers()
        {
            var users = await _userCollection.Find(u=>u.Role == RoleType.Traveler).ToListAsync();
            return users;
        }

        //get user by id
        public async Task<User> GetUserById(string id)
        {
            var user =  await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        //update user
        public async Task<bool> UpdateUser(User user)
        {
            user.Password =  BCryptNet.HashPassword(user.Password);
            var res = await _userCollection.ReplaceOneAsync(x => x.Id == user.Id, user);

            return true;
        }
    }
}

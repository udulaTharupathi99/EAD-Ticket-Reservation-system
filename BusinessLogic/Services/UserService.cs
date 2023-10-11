﻿using EAD_APP.BusinessLogic.Interfaces;
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
        
        public async Task<User> LoginUser(LoginRequest request)
        {
            var user =  await _userCollection.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
            
            if (user != null)
            {
                var res = BCryptNet.Verify(request.Password, user.Password);

                if (!res)
                {
                    throw new Exception("Incorrect credentials.");
                }
                
                if (user.Status == ActiveStatus.Delete)
                {
                    throw new Exception("User deactivated.");
                }
            }
            else
            {
                throw new Exception("User deleted.");
            }
            
            return user;
        }

        public async Task<bool> UpdateStatus(User user, ActiveStatus status)
        {
            user.Status = status;
            var res = await _userCollection.ReplaceOneAsync(x => x.Id == user.Id, user);

            return true;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var res = await _userCollection.DeleteOneAsync(x => x.Id == id);
            return true;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _userCollection.Find(_ => true).ToListAsync();
            return users;
        }

        public async Task<User> GetUserById(string id)
        {
            var user =  await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> UpdateUser(User user)
        {
            user.Password =  BCryptNet.HashPassword(user.Password);
            var res = await _userCollection.ReplaceOneAsync(x => x.Id == user.Id, user);

            return true;
        }
    }
}

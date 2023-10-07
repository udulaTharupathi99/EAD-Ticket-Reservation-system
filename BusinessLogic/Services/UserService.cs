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

        public async Task<bool> CreateUser(User user)
        {
            //check NIC
            
            user.Password = BCryptNet.HashPassword(user.Password);
            await _userCollection.InsertOneAsync(user);
            return true;
        }
        
        public async Task<bool> LoginUser(LoginRequest request)
        {
            var user =  await _userCollection.Find(u => u.Email == request.Email).FirstOrDefaultAsync();

            if (user == null || user.Status == ActiveStatus.Delete)
            {
                return false;
            }

            var res = BCryptNet.Verify(request.Password, user.Password);

            if (!res)
            {
                return false;
            }

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
            var user =  await _userCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var res = await _userCollection.ReplaceOneAsync(x => x.Id == user.Id, user);

            return true;
        }
    }
}

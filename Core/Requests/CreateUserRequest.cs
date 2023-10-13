////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: CreateUserRequest.cs
//Author : IT20135102
//Created On : 9/10/2023 
//Description : CreateUser Request
////////////////////////////////////////////////////////////////////////////////////////////////////////
using EAD_APP.Core.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EAD_APP.Core.Requests
{
    public class CreateUserRequest
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("nic")]
        public int NIC { get; set; }

        [BsonElement("role")]
        public RoleType Role { get; set; }

        [BsonElement("status")]
        public ActiveStatus Status { get; set; }
    }
}

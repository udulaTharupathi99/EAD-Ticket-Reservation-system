using EAD_APP.Core.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EAD_APP.Core.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("nic")]
        public string NIC { get; set; }

        [BsonElement("role")]
        public RoleType Role { get; set; }

        [BsonElement("status")]
        public ActiveStatus Status { get; set; }
    }
}

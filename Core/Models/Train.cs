﻿using EAD_APP.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_APP.Core.Models
{
    [BsonIgnoreExtraElements]
    public class Train
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("trainName")]
        public string TrainName { get; set; }
        
        [BsonElement("note")]
        public string Note { get; set; }

        [BsonElement("status")]
        public ActiveStatus Status { get; set; }
    }
}

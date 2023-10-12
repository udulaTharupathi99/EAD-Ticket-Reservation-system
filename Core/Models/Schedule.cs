using EAD_APP.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_APP.Core.Models;

public class Schedule
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("trainId")]
    public string TrainId { get; set; }
    
    [BsonElement("trainName")]
    public string TrainName { get; set; }

    [BsonElement("start")]
    public string Start { get; set; }

    [BsonElement("destination")]
    public string Destination { get; set; }

    [BsonElement("StartDateTime")]
    public DateTime StartDateTime { get; set; }

    [BsonElement("destinationDateTime")]
    public DateTime DestinationDateTime { get; set; }

    [BsonElement("status")]
    public ActiveStatus Status { get; set; }
}
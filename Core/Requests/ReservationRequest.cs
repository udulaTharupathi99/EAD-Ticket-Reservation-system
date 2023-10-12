using EAD_APP.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_APP.Core.Requests;

public class ReservationRequest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("scheduleId")]
    public string ScheduleId { get; set; }
    //public string TrainId { get; set; }
    
    [BsonElement("travelerNIC")]
    public string TravelerNIC { get; set; }
    
    [BsonElement("bookingDateTime")]
    public DateTime BookingDateTime { get; set; }
    
    [BsonElement("status")]
    public ActiveStatus Status { get; set; }
    
    [BsonElement("seats")]
    public int Seats  { get; set; }
    
    //public Schedule Schedule { get; set; }
}
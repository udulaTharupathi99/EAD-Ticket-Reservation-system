////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Reservation.cs
//Author : IT20124526
//Created On : 9/10/2023 
//Description : Reservation
////////////////////////////////////////////////////////////////////////////////////////////////////////
using EAD_APP.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_APP.Core.Models;

public class Reservation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("trainId")]
    public string TrainId { get; set; }

    [BsonElement("scheduleId")]
    public string ScheduleId { get; set; }
    
    [BsonElement("travelerNIC")]
    public string TravelerNIC { get; set; }

    [BsonElement("bookingDateTime")]
    public DateTime BookingDateTime { get; set; }
    
    [BsonElement("status")]
    public ActiveStatus Status { get; set; }
    
    [BsonElement("seats")]
    public int Seats  { get; set; }
    
    [BsonElement("schedule")]
    public Schedule Schedule { get; set; }
    
    [BsonElement("isPast")]
    public bool IsPast { get; set; }
}
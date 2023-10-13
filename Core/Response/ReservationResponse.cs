////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ReservationResponse.cs
//Author : IT20135102
//Created On : 9/10/2023 
//Description : Reservation Response 
////////////////////////////////////////////////////////////////////////////////////////////////////////
using EAD_APP.Core.Enums;

namespace EAD_APP.Core.Response;

public class ReservationResponse
{
    public string ReservationId { get; set; }
    public string TrainId { get; set; }
    public string TravelerNIC { get; set; }
    public DateTime BookingDateTime { get; set; }
    public ActiveStatus Status { get; set; }
    
    public int Seats  { get; set; }
    
    public string ScheduleId { get; set; }
    public string Start { get; set; }
    public string Destination { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime DestinationDateTime { get; set; }
    
    public string TrainName { get; set; }
    public bool IsPast { get; set; }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: IReservationService.cs
//Author : IT20124526
//Created On : 9/10/2023 
//Description : Interface for reservation service 
////////////////////////////////////////////////////////////////////////////////////////////////////////
using EAD_APP.Core.Models;
using EAD_APP.Core.Requests;
using EAD_APP.Core.Response;

namespace EAD_APP.BusinessLogic.Interfaces;

public interface IReservationService
{
    Task<List<Reservation>> GetAllReservation();
    Task<Reservation> GetReservationById(string id);
    Task<bool> CreateReservation(ReservationRequest reservation);
    Task<bool> UpdateReservation(ReservationRequest reservation);
    Task<bool> DeleteReservation(string id);
    
    Task<List<Reservation>> GetAllReservationByTravelerId(string userId);
}
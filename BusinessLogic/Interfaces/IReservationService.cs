using EAD_APP.Core.Models;

namespace EAD_APP.BusinessLogic.Interfaces;

public interface IReservationService
{
    Task<List<Reservation>> GetAllReservation();
    Task<Reservation> GetReservationById(string id);
    Task<bool> CreateReservation(Reservation reservation);
    Task<bool> UpdateReservation(Reservation reservation);
    Task<bool> DeleteReservation(string id);
    
    Task<List<Reservation>> GetAllReservationByTravelerId(string userId);
}
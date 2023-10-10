using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Models;
using MongoDB.Driver;

namespace EAD_APP.BusinessLogic.Services;

public class ReservationService : IReservationService
{
    private readonly IMongoCollection<Reservation> _reservationCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Schedule> _scheduleCollection;

    public ReservationService(IMongoDatabase mongoDatabase)
    {
        _reservationCollection = mongoDatabase.GetCollection<Reservation>("reservation");
        _userCollection = mongoDatabase.GetCollection<User>("user");
        _scheduleCollection = mongoDatabase.GetCollection<Schedule>("schedule");
    }

    public async Task<List<Reservation>> GetAllReservation()
    {
        var trains = await _reservationCollection.Find(_ => true).ToListAsync();
        return trains;
    }

    public async Task<Reservation> GetReservationById(string id)
    {
        var reservation =  await _reservationCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        var user = await _userCollection.Find(t => t.Id == reservation.TravelerId).FirstOrDefaultAsync();
        return reservation;
    }

    public async Task<bool> CreateReservation(Reservation reservation)
    {
        var allReservationsForUser = await _reservationCollection.Find(s => s.TravelerId == reservation.TravelerId).ToListAsync();

        var futureReservations = new List<Reservation>();
        if (allReservationsForUser.Count >= 4)
        {
            foreach (var reservationModel in allReservationsForUser)
            {
                if (reservationModel.BookingDateTime > DateTime.Now)
                {
                    futureReservations.Add(reservationModel);
                }

                if (futureReservations.Count >= 4)
                {
                    throw new Exception("Maximum 4 reservations per reference ID.");
                }
            }
        }
        
        await _reservationCollection.InsertOneAsync(reservation);
        return true;
    }

    public async Task<bool> UpdateReservation(Reservation reservation)
    {
        var schedule =  await _scheduleCollection.Find(t => t.Id == reservation.ScheduleId).FirstOrDefaultAsync();

        if (!((schedule.StartDateTime - DateTime.Now).Days >= 5))
        {
            throw new Exception("At least 5 days before the reservation date.");
        }
        var res = await _reservationCollection.ReplaceOneAsync(x => x.Id == reservation.Id, reservation);
        return true;
    }

    public async Task<bool> DeleteReservation(string id)
    {
        var reservation =  await _reservationCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        var schedule =  await _scheduleCollection.Find(t => t.Id == reservation.ScheduleId).FirstOrDefaultAsync();

        if (!((schedule.StartDateTime - DateTime.Now).Days >= 5))
        {
            throw new Exception("At least 5 days before the reservation date.");
        }
        var res = await _reservationCollection.DeleteOneAsync(x => x.Id == id);
        return true;
    }

    public async Task<List<Reservation>> GetAllReservationByTravelerId(string userId)
    {
        var trains = await _reservationCollection.Find(s => s.TravelerId == userId).ToListAsync();
        return trains;
    }
}
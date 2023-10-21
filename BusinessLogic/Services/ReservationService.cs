////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ReservationService.cs
//Author : IT20124526
//Created On : 9/10/2023 
//Description : ReservationService service 
////////////////////////////////////////////////////////////////////////////////////////////////////////
using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Enums;
using EAD_APP.Core.Models;
using EAD_APP.Core.Requests;
using EAD_APP.Core.Response;
using MongoDB.Driver;

namespace EAD_APP.BusinessLogic.Services;

public class ReservationService : IReservationService
{
    private readonly IMongoCollection<Reservation> _reservationCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Schedule> _scheduleCollection;
    private readonly IMongoCollection<Train> _trainCollection;

    public ReservationService(IMongoDatabase mongoDatabase)
    {
        _reservationCollection = mongoDatabase.GetCollection<Reservation>("reservation");
        _userCollection = mongoDatabase.GetCollection<User>("user");
        _scheduleCollection = mongoDatabase.GetCollection<Schedule>("schedule");
        _trainCollection = mongoDatabase.GetCollection<Train>("train");
    }

    //get all reservations
    public async Task<List<Reservation>> GetAllReservation()
    {
        var trains = await _reservationCollection.Find(_ => true).ToListAsync();
        return trains;
    }

    //get reservation by id
    public async Task<Reservation> GetReservationById(string id)
    {
        
        var reservation =  await _reservationCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        var isPastSchedule = await CheckIsPastReservation(reservation.Schedule.StartDateTime);

        reservation.IsPast = isPastSchedule;
        
        return reservation;
    }

    //add new reservation
    public async Task<bool> CreateReservation(ReservationRequest reservation)
    {
        var user = await _userCollection.Find(t => t.NIC == reservation.TravelerNIC).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new Exception("Invalid Traveler !. Plz enter the correct NIC.");
        }
        var schedule =  await _scheduleCollection.Find(t => t.Id == reservation.ScheduleId).FirstOrDefaultAsync();
        
        var allReservationsForUser = await _reservationCollection.Find(s => s.TravelerNIC == reservation.TravelerNIC).ToListAsync();

        var futureReservations = new List<Reservation>();
        if (allReservationsForUser.Count != 0)
        {
            if (allReservationsForUser.Count >= 4)
            {
                foreach (var reservationModel in allReservationsForUser)
                {
                    if (reservationModel.Schedule.StartDateTime > DateTime.Now)
                    {
                        futureReservations.Add(reservationModel);
                    }

                }

                if (futureReservations.Count >= 4)
                {
                    throw new Exception("Maximum 4 reservations per reference ID.");
                }
            }
        }

        var resModel = new Reservation()
        {
            Id = reservation.Id,
            TrainId = schedule.TrainId,
            ScheduleId = reservation.ScheduleId,
            TravelerNIC = reservation.TravelerNIC,
            BookingDateTime = DateTime.Now,
            Seats = reservation.Seats,
            Status = reservation.Status,
            Schedule = schedule,
            IsPast = false
        };
        
        await _reservationCollection.InsertOneAsync(resModel);
        return true;
    }

    //update reservation
    public async Task<bool> UpdateReservation(ReservationRequest reservation)
    {
        var schedule =  await _scheduleCollection.Find(t => t.Id == reservation.ScheduleId).FirstOrDefaultAsync();

        if (!((schedule.StartDateTime - DateTime.Now).Days >= 5))
        {
            throw new Exception("Changes can only be made up to 5 days before the travel start date");
        }
        
        var reservationModel =  await _reservationCollection.Find(t => t.Id == reservation.Id).FirstOrDefaultAsync();
        reservationModel.Seats = reservation.Seats;
        reservationModel.BookingDateTime = DateTime.Now;
        
        var res = await _reservationCollection.ReplaceOneAsync(x => x.Id == reservation.Id, reservationModel);
        return true;
    }

    //delete reservation
    public async Task<bool> DeleteReservation(string id)
    {
        var reservation =  await _reservationCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        var schedule =  await _scheduleCollection.Find(t => t.Id == reservation.ScheduleId).FirstOrDefaultAsync();

        if (!((schedule.StartDateTime - DateTime.Now).Days >= 5))
        {
            throw new Exception("Changes can only be made up to 5 days before the travel start date");
        }
        var res = await _reservationCollection.DeleteOneAsync(x => x.Id == id);
        return true;
    }

    //get reservations by user NIC
    public async Task<List<Reservation>> GetAllReservationByTravelerId(string userNIC)
    {
        var reservations = await _reservationCollection.Find(s => s.TravelerNIC == userNIC).ToListAsync();

        foreach (var reservation in reservations)
        {
            reservation.IsPast = await CheckIsPastReservation(reservation.Schedule.StartDateTime);

            var maxDate = DateTime.Now.AddDays(5);
            if (reservation.Schedule.StartDateTime < maxDate)
            {
                reservation.Status = ActiveStatus.Delete;
            }
        }
        return reservations;
    }

    private async Task<bool> CheckIsPastReservation(DateTime dateTime)
    {
        var res = dateTime < DateTime.Now;
        if (res)
        {
            return true;
        }
        return false;
    }
}
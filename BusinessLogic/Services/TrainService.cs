using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Enums;
using EAD_APP.Core.Models;
using MongoDB.Driver;

namespace EAD_APP.BusinessLogic.Services;

public class TrainService : ITrainService
{
    private readonly IMongoCollection<Train> _trainCollection;
    private readonly IMongoCollection<Schedule> _scheduleCollection;
    private readonly IMongoCollection<Reservation> _reservationCollection;

    public TrainService(IMongoDatabase mongoDatabase)
    {
        _trainCollection = mongoDatabase.GetCollection<Train>("train");
        _scheduleCollection = mongoDatabase.GetCollection<Schedule>("schedule");
        _reservationCollection = mongoDatabase.GetCollection<Reservation>("reservation");
    }

    public async Task<List<Train>> GetAllTrains()
    {
        var trains = await _trainCollection.Find(_ => true).ToListAsync();
        return trains;
    }

    public async Task<Train> GetTrainById(string id)
    {
        var train =  await _trainCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        return train;
    }

    public async Task<bool> CreateTrain(Train train)
    {
        await _trainCollection.InsertOneAsync(train);
        return true;
    }

    public async Task<bool> UpdateTrain(Train train)
    {
        var res = await _trainCollection.ReplaceOneAsync(x => x.Id == train.Id, train);
        return true;
    }

    public async Task<bool> DeleteTrain(string id)
    {
        var res = await _trainCollection.DeleteOneAsync(x => x.Id == id);
        return true;
    }

    public async Task<bool> UpdateStatus(Train train, ActiveStatus status)
    {
        if (status == ActiveStatus.Delete)
        {
            //delete schedules
            var isDeleted = await ChangeSchedulesStatusByTrainId(train.Id);

            if (!isDeleted)
            {
                throw new Exception("Can't delete this train.");

            }
        }
        
        train.Status = status;
        var res = await _trainCollection.ReplaceOneAsync(x => x.Id == train.Id, train);
        return true;
    }

    private async Task<bool> ChangeSchedulesStatusByTrainId(string trainId)
    {
        var schedules = await _scheduleCollection.Find(s => s.TrainId == trainId).ToListAsync();
        var reservations = await _reservationCollection.Find(s => s.TrainId == trainId).ToListAsync();

        var isFuture = false;
        foreach (var reservation in reservations)
        {
            foreach (var schedule in schedules)
            {
                if (reservation.ScheduleId == schedule.Id)
                {
                    isFuture = schedule.StartDateTime > DateTime.Now;
                    if (isFuture)
                    {
                        break;
                    }
                }
            }
        }

        if (isFuture)
        {
            return false;
        }
        
        foreach (var schedule in schedules)
        {
            schedule.Status = ActiveStatus.Delete;
            var res = await _scheduleCollection.ReplaceOneAsync(x => x.Id == schedule.Id, schedule);
        }
        return true;
    }
}
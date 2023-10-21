////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: TrainService.cs
//Author : IT20151188
//Created On : 9/10/2023 
//Description : TrainService service 
////////////////////////////////////////////////////////////////////////////////////////////////////////
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

    //get all trains
    public async Task<List<Train>> GetAllTrains()
    {
        var trains = await _trainCollection.Find(_ => true).ToListAsync();

        // var trainList = new List<Train>();
        // foreach (var train in trains)
        // {
        //     if (train.Status == ActiveStatus.Active)
        //     {
        //         trainList.Add(train);
        //     }
        // }
        return trains;
    }

    //get train by id
    public async Task<Train> GetTrainById(string id)
    {
        var train =  await _trainCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        return train;
    }

    //add new train
    public async Task<bool> CreateTrain(Train train)
    {
        train.Status = ActiveStatus.Delete;
        await _trainCollection.InsertOneAsync(train);
        return true;
    }

    //update train
    public async Task<bool> UpdateTrain(Train train)
    {
        var res = await _trainCollection.ReplaceOneAsync(x => x.Id == train.Id, train);
        return true;
    }

    //delete train
    //change train status(deactivate)
    public async Task<bool> DeleteTrain(string id)
    {
        var train =  await _trainCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        var isDeleted = await ChangeSchedulesStatusByTrainId(train.Id);

        if (!isDeleted)
        {
            throw new Exception("Can't cancel this train. Train has future reservations");
        }

        train.Status = ActiveStatus.Delete;
        var res = await _trainCollection.ReplaceOneAsync(x => x.Id == train.Id, train);
        //var res = await _trainCollection.DeleteOneAsync(x => x.Id == id);
        return true;
    }

    //change train status(active)
    public async Task<bool> UpdateStatus(string id)
    {
        var train =  await _trainCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        var schedules = await _scheduleCollection.Find(s => s.TrainId == id).ToListAsync();
        
        train.Status  = ActiveStatus.Active;
        var res = await _trainCollection.ReplaceOneAsync(x => x.Id == train.Id, train);

        foreach (var schedule in schedules)
        {
            schedule.Status = ActiveStatus.Active;
            var res1 = await _scheduleCollection.ReplaceOneAsync(x => x.Id == schedule.Id, schedule);
        }
        return true;
    }

    private async Task<bool> ChangeSchedulesStatusByTrainId(string trainId)
    {
        var schedules = await _scheduleCollection.Find(s => s.TrainId == trainId).ToListAsync();
        var reservations = await _reservationCollection.Find(s => s.TrainId == trainId).ToListAsync();

        var isFuture = false;
        foreach (var reservation in reservations)
        {
            isFuture = reservation.Schedule.StartDateTime > DateTime.Now;
            if (isFuture)
            {
                break;
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
            //var res = await _scheduleCollection.DeleteOneAsync(x => x.Id == schedule.Id);
        }
        return true;
    }
}
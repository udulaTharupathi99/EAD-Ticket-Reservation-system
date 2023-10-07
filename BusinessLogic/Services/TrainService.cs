using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Models;
using MongoDB.Driver;

namespace EAD_APP.BusinessLogic.Services;

public class TrainService : ITrainService
{
    private readonly IMongoCollection<Train> _trainCollection;

    public TrainService(IMongoDatabase mongoDatabase)
    {
        _trainCollection = mongoDatabase.GetCollection<Train>("train");
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
}
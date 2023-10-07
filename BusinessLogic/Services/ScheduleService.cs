using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Models;
using MongoDB.Driver;

namespace EAD_APP.BusinessLogic.Services;

public class ScheduleService : IScheduleService
{
    private readonly IMongoCollection<Schedule> _scheduleCollection;

    public ScheduleService(IMongoDatabase mongoDatabase)
    {
        _scheduleCollection = mongoDatabase.GetCollection<Schedule>("schedule");
    }

    public async Task<List<Schedule>> GetAllSchedule()
    {
        var trains = await _scheduleCollection.Find(_ => true).ToListAsync();
        return trains;
    }

    public async Task<Schedule> GetScheduleById(string id)
    {
        var train =  await _scheduleCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        return train;
    }

    public async Task<bool> CreateSchedule(Schedule schedule)
    {
        await _scheduleCollection.InsertOneAsync(schedule);
        return true;
    }

    public async Task<bool> UpdateSchedule(Schedule schedule)
    {
        var res = await _scheduleCollection.ReplaceOneAsync(x => x.Id == schedule.Id, schedule);
        return true;
    }

    public async Task<bool> DeleteSchedule(string id)
    {
        var res = await _scheduleCollection.DeleteOneAsync(x => x.Id == id);
        return true;
    }

    public async Task<List<Schedule>> GetAllScheduleByTrainId(string trainId)
    {
        var trains = await _scheduleCollection.Find(s => s.TrainId == trainId).ToListAsync();
        return trains;
    }
}
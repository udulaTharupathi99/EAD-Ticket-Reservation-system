////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ScheduleService.cs
//Author : IT20135102
//Created On : 9/10/2023 
//Description : scheduleService service 
////////////////////////////////////////////////////////////////////////////////////////////////////////
using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Enums;
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

    //get all Schedules
    public async Task<List<Schedule>> GetAllSchedule()
    {
        var schedules = await _scheduleCollection.Find(s => s.Status == ActiveStatus.Active).ToListAsync();
        //var trains = await _scheduleCollection.Find(s => s.Status == ActiveStatus.Active).ToListAsync();

        var schedulesList = new List<Schedule>();
        foreach (var schedule in schedules)
        {
            var maxFutureDate = DateTime.Now.AddDays(30); 
            if ((schedule.StartDateTime > DateTime.Now) && schedule.StartDateTime < maxFutureDate)
            {
                schedulesList.Add(schedule);
                
            }
        }
        return schedulesList;
    }

    //get Schedule by id
    public async Task<Schedule> GetScheduleById(string id)
    {
        var train =  await _scheduleCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        return train;
    }

    //add new Schedule
    public async Task<bool> CreateSchedule(Schedule schedule)
    {
        schedule.Status = ActiveStatus.Delete;
        await _scheduleCollection.InsertOneAsync(schedule);
        return true;
    }

    //Update Schedule
    public async Task<bool> UpdateSchedule(Schedule schedule)
    {
        var res = await _scheduleCollection.ReplaceOneAsync(x => x.Id == schedule.Id, schedule);
        return true;
    }

    //delete Schedule
    public async Task<bool> DeleteSchedule(string id)
    {
        var res = await _scheduleCollection.DeleteOneAsync(x => x.Id == id);
        return true;
    }

    //get Schedule by train id
    public async Task<List<Schedule>> GetAllScheduleByTrainId(string trainId)
    {
        var trains = await _scheduleCollection.Find(s => s.TrainId == trainId).ToListAsync();
        return trains;
    }
}
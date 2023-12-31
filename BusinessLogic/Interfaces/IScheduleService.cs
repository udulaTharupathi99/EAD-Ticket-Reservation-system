﻿////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: IScheduleService.cs
//Author : IT20135102
//Created On : 9/10/2023 
//Description : Interface for scheduleService service 
////////////////////////////////////////////////////////////////////////////////////////////////////////
using EAD_APP.Core.Models;

namespace EAD_APP.BusinessLogic.Interfaces;

public interface IScheduleService
{
    Task<List<Schedule>> GetAllSchedule();
    Task<Schedule> GetScheduleById(string id);
    Task<bool> CreateSchedule(Schedule user);
    Task<bool> UpdateSchedule(Schedule user);
    Task<bool> DeleteSchedule(string id);
    
    Task<List<Schedule>> GetAllScheduleByTrainId(string trainId);
}
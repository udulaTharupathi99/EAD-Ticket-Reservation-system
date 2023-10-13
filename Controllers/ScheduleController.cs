////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ScheduleController.cs
//Author : IT20135102
//Created On : 9/10/2023 
//Description : ScheduleController
////////////////////////////////////////////////////////////////////////////////////////////////////////
using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Models;
using EAD_APP.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace EAD_APP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : Controller
{
    public readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    //get all Schedules
    [HttpGet]
    public async Task<IActionResult> GetAllSchedules()
    {
        var schedule = await _scheduleService.GetAllSchedule();
        return Ok(schedule);
    }
    
    //get Schedule by id
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetScheduleById(string id)
    {
        var schedule = await _scheduleService.GetScheduleById(id);
        if (schedule == null)
        {
            return NotFound();
        }
    
        return Ok(schedule);
    }
    
    //add new Schedule
    [HttpPost]
    public async Task<IActionResult> AddSchedule(Schedule request)
    {
        await _scheduleService.CreateSchedule(request);
        return CreatedAtAction(nameof(GetScheduleById), new { id = request.Id }, request);
    }
    
    //Update Schedule
    [HttpPut]
    public async Task<IActionResult> UpdateSchedule(Schedule request)
    {
        var schedule = await _scheduleService.GetScheduleById(request.Id);
        if (schedule == null)
        {
            return NotFound();
        }
    
        await _scheduleService.UpdateSchedule(request);
        var newSchedule = await _scheduleService.GetScheduleById(request.Id);
        return Ok(newSchedule);
    }
    
    //delete Schedule
    [HttpDelete]
    public async Task<IActionResult> DeleteSchedule(string id)
    {
        var schedule = await _scheduleService.GetScheduleById(id);
        if (schedule == null)
        {
            return NotFound();
        }
    
        await _scheduleService.DeleteSchedule(id);
        var res = new ApiResponse() { IsSuccess = "true", Msg = "Success" };
        return Ok(res);
    }
    
    //get Schedule by train id
    [HttpGet]
    [Route("train/{trainId}")]
    public async Task<IActionResult> GetAllScheduleByTrainId(string trainId)
    {
        var schedule = await _scheduleService.GetAllScheduleByTrainId(trainId);
        return Ok(schedule);
    }
}
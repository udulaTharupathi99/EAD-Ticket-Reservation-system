////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: TrainController.cs
//Author : IT20151188
//Created On : 9/10/2023 
//Description : TrainController
////////////////////////////////////////////////////////////////////////////////////////////////////////
using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Enums;
using EAD_APP.Core.Models;
using EAD_APP.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace EAD_APP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainController : Controller
{
    public readonly ITrainService _trainService;

    public TrainController(ITrainService trainService)
    {
        _trainService = trainService;
    }

    //get all trains
    [HttpGet]
    public async Task<IActionResult> GetAllTrains()
    {
        var trains = await _trainService.GetAllTrains();
        return Ok(trains);
    }
    
    //get train by id
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetTrainById(string id)
    {
        var train = await _trainService.GetTrainById(id);
        if (train == null)
        {
            return NotFound();
        }
    
        return Ok(train);
    }
    
    //add new train
    [HttpPost]
    public async Task<IActionResult> AddTrain(Train request)
    {
        await _trainService.CreateTrain(request);
        return CreatedAtAction(nameof(GetTrainById), new { id = request.Id }, request);
    }
    
    //update train
    [HttpPut]
    public async Task<IActionResult> UpdateTrain(Train request)
    {
        var train = await _trainService.GetTrainById(request.Id);
        if (train == null)
        {
            return NotFound();
        }
    
        await _trainService.UpdateTrain(request);
        var newTrain = await _trainService.GetTrainById(train.Id);
        return Ok(newTrain);
    }
    
    //delete train
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteTrain(string id)
    {
        var train = await _trainService.GetTrainById(id);
        if (train == null)
        {
            return NotFound();
        }
    
        await _trainService.DeleteTrain(id);
        var res = new ApiResponse() { IsSuccess = "true", Msg = "Success" };
        return Ok(res);
    }
    
    //change train status(active, deactivate)
    [HttpPut]
    [Route("{trainId}/{status}")]
    public async Task<IActionResult> UpdateStatus(string trainId, ActiveStatus status)
    {
        try
        {
            var train = await _trainService.GetTrainById(trainId);
            if (train == null)
            {
                return NotFound();
            }

            await _trainService.UpdateStatus(train, status);
            var res = new ApiResponse() { IsSuccess = "true", Msg = "Success"};
            return Ok(res);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
       
    }
}
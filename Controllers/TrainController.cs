using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Models;
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

    [HttpGet]
    public async Task<IActionResult> GetAllTrains()
    {
        var trains = await _trainService.GetAllTrains();
        return Ok(trains);
    }
    
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
    
    [HttpPost]
    public async Task<IActionResult> AddTrain(Train request)
    {
        await _trainService.CreateTrain(request);
        return CreatedAtAction(nameof(GetTrainById), new { id = request.Id }, request);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateTrain(Train request)
    {
        var train = await _trainService.GetTrainById(request.Id);
        if (train == null)
        {
            return NotFound();
        }
    
        await _trainService.UpdateTrain(request);
        return NoContent();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteTrain(string id)
    {
        var train = await _trainService.GetTrainById(id);
        if (train == null)
        {
            return NotFound();
        }
    
        await _trainService.DeleteTrain(id);
        return NoContent();
    }
}
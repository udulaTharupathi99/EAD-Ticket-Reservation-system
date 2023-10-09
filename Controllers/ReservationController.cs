using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Models;
using EAD_APP.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace EAD_APP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : Controller
{
    public readonly IReservationService _ReservationService;

    public ReservationController(IReservationService reservationService)
    {
        _ReservationService = reservationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllReservation()
    {
        var schedule = await _ReservationService.GetAllReservation();
        return Ok(schedule);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetReservationById(string id)
    {
        var schedule = await _ReservationService.GetReservationById(id);
        if (schedule == null)
        {
            return NotFound();
        }
    
        return Ok(schedule);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddReservation(Reservation request)
    {
        await _ReservationService.CreateReservation(request);
        return CreatedAtAction(nameof(GetReservationById), new { id = request.Id }, request);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateReservation(Reservation request)
    {
        var schedule = await _ReservationService.GetReservationById(request.Id);
        if (schedule == null)
        {
            return NotFound();
        }
    
        await _ReservationService.UpdateReservation(request);
        var newSchedule = await _ReservationService.GetReservationById(request.Id);
        return Ok(newSchedule);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteReservation(string id)
    {
        var schedule = await _ReservationService.GetReservationById(id);
        if (schedule == null)
        {
            return NotFound();
        }
    
        await _ReservationService.DeleteReservation(id);
        var res = new ApiResponse() { IsSuccess = "true", Msg = "Success" };
        return Ok(res);
    }
    
    [HttpGet]
    [Route("reservation/{userId}")]
    public async Task<IActionResult> GetAllReservationByUserId(string userId)
    {
        var schedule = await _ReservationService.GetAllReservationByTravelerId(userId);
        return Ok(schedule);
    }
}
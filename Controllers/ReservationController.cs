using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Models;
using EAD_APP.Core.Requests;
using EAD_APP.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace EAD_APP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllReservation()
    {
        var schedule = await _reservationService.GetAllReservation();
        return Ok(schedule);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetReservationById(string id)
    {
        var schedule = await _reservationService.GetReservationById(id);
        if (schedule == null)
        {
            return NotFound();
        }
    
        return Ok(schedule);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddReservation(ReservationRequest request)
    {
        try
        {
            await _reservationService.CreateReservation(request);
            
            // var ob = new Reservation();
            // return CreatedAtAction(nameof(GetReservationById), new { id = request.Id }, ob);
            // var schedule = await _reservationService.GetReservationById(request.Id);
            
            var res = new ApiResponse() { IsSuccess = "true", Msg = "Success"};
            return Ok(res);
        }
        catch (Exception e)
        {
            var res = new ApiResponse() { IsSuccess = "false", Msg = e.Message };
            return BadRequest(res);
        }
        
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateReservation(ReservationRequest request)
    {
        try
        {
            var schedule = await _reservationService.GetReservationById(request.Id);
            if (schedule == null)
            {
                return NotFound();
            }
    
            await _reservationService.UpdateReservation(request);
            var newSchedule = await _reservationService.GetReservationById(request.Id);
            return Ok(newSchedule);

        }
        catch (Exception e)
        {
            var res = new ApiResponse() { IsSuccess = "false", Msg = e.Message };
            return BadRequest(res);
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteReservation(string id)
    {
        try
        {
            var schedule = await _reservationService.GetReservationById(id);
            if (schedule == null)
            {
                return NotFound();
            }
    
            await _reservationService.DeleteReservation(id);
            var res = new ApiResponse() { IsSuccess = "true", Msg = "Success" };
            return Ok(res);

        }
        catch (Exception e)
        {
            var res = new ApiResponse() { IsSuccess = "false", Msg = e.Message };
            return BadRequest(res);
        }
        
    }
    
    [HttpGet]
    [Route("traverler/{userNIC}")]
    public async Task<IActionResult> GetAllReservationByUserNIC(string userNIC)
    {
        var schedule = await _reservationService.GetAllReservationByTravelerId(userNIC);
        return Ok(schedule);
    }
}
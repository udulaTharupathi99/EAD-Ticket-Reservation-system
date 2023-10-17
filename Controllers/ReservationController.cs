////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: UserController.cs
//Author : IT20124526
//Created On : 9/10/2023 
//Description : UserController
////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    
    //get all reservations
    [HttpGet]
    public async Task<IActionResult> GetAllReservation()
    {
        var schedule = await _reservationService.GetAllReservation();
        return Ok(schedule);
    }
    
    //get reservation by id
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
    
    //add new reservation
    [HttpPost]
    public async Task<IActionResult> AddReservation(ReservationRequest request)
    {
        try
        {
            await _reservationService.CreateReservation(request);
            
            var res = new ApiResponse() { IsSuccess = "true", Msg = "Success"};
            return Ok(res);
        }
        catch (Exception e)
        {
            var res = new ApiResponse() { IsSuccess = "false", Msg = e.Message };
            return BadRequest(res);
        }
        
    }
    
    //update reservation
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
    
    //delete reservation
    [HttpDelete]
    [Route("{id}")]
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
    
    //get reservations by user NIC
    [HttpGet]
    [Route("traverler/{userNIC}")]
    public async Task<IActionResult> GetAllReservationByUserNIC(string userNIC)
    {
        var schedule = await _reservationService.GetAllReservationByTravelerId(userNIC);
        return Ok(schedule);
    }
}
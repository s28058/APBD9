using APBD9.DTOs;
using APBD9.Exceptions;
using APBD9.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD9.Controllers;

[Route("api/[controller]")]
[ApiController]

public class TripController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _tripService.GetTripsAsync(page, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "server error");
        }
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        try
        {
            await _tripService.DeleteClientAsync(idClient);
        }
        catch (NoSuchClientException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "no such client");

        }
        catch (ClientHasTripsException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "client has trips");

        }

        return Ok();
    }
    
    [HttpPost("{idTrip}/clients")]

    public async Task<IActionResult> AssignClientToTripAsync(ClientRequestDTO clientRequest)
    {
        try
        {
            await _tripService.AssignClientToTripAsync(clientRequest);
            return Ok();
        }
        catch (NoSuchTripException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "no such trip");
        }
        catch (ClientAlreadyExistsException exc)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "client already exists");
        }
        catch (ClientAlreadyOnTripException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "client already on this trip");
        }
        catch (TripAlreadyStartedException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "trip already started");
        }
        
    }
}
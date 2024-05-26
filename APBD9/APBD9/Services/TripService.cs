using APBD9.Context;
using APBD9.DTOs;
using Microsoft.EntityFrameworkCore;

namespace APBD9.Services;

public class TripService : ITripService
{
    private readonly MasterContext _context;
    public TripService(MasterContext context)
    {
        _context = context;
    }
    
    public async Task<TripRequestDTO> GetTripsAsync(int page, int pageSize)
    {
        var pageIndex = page - 1;
        var tripsAmount = await _context.Trips.CountAsync();
        var pagesAmount = (int)Math.Ceiling(tripsAmount / (double)pageSize);
        var trips = await _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Select(t => new TripDTO(
                t.Name,
                t.Description,
                t.DateFrom.ToString("yyyy-MM-dd"),
                t.DateTo.ToString("yyyy-MM-dd"),
                t.MaxPeople,
                t.IdCountries.Select(c => new CountryDTO(c.Name)).ToList(),
                t.ClientTrips
                    .Select(ct => new ClientDTO(ct.IdClientNavigation.FirstName, ct.IdClientNavigation.LastName))
                    .ToList()
            )).ToListAsync();

        return new TripRequestDTO(
            page,
            pageSize,
            pagesAmount,
            trips);

    }

    public async Task<bool> DeleteClientAsync(int clientId)
    {
        return true;
    }

    public async Task<bool> AssignClientToTripAsync(ClientRequestDTO clientRequest)
    {
        return true;
    }
}
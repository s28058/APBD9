using APBD9.Context;
using APBD9.DTOs;
using APBD9.Exceptions;
using APBD9.Models;
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
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == clientId);

        if (client == null)
        {
            throw new NoSuchClientException();
        }

        if (client.ClientTrips.Any())
        {
            throw new ClientHasTripsException();
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> AssignClientToTripAsync(ClientRequestDTO clientRequest)
    {
        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == clientRequest.IdTrip);
        if (trip == null)
        {
            throw new NoSuchTripException();
        }

        if (trip.DateFrom <= DateTime.Now)
        {
            throw new TripAlreadyStartedException();
        }

        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientRequest.Pesel);
        if (existingClient != null)
        {
            throw new ClientAlreadyExistsException();
        }

        var clientWithThisPeselOnTrip = await _context.ClientTrips
            .Include(ct => ct.IdClientNavigation)
            .Select(ct => ct.IdClientNavigation)
            .Where(c => c.Pesel == clientRequest.Pesel)
            .FirstOrDefaultAsync();

        if (clientWithThisPeselOnTrip != null)
        {
            throw new ClientAlreadyOnTripException();
        }

        await _context.Clients.AddAsync(
            new Client
            {
                FirstName = clientRequest.FirstName,
                LastName = clientRequest.LastName,
                Email = clientRequest.Email,
                Telephone = clientRequest.Telephone,
                Pesel = clientRequest.Pesel

            });

        await _context.ClientTrips.AddAsync(
            new ClientTrip
            {
                IdTrip = clientRequest.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientRequest.PaymentDate,
            });

        await _context.SaveChangesAsync();
        return true;
    }
}
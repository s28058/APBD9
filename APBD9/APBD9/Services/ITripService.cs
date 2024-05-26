using APBD9.DTOs;

namespace APBD9.Services;

public interface ITripService
{
    Task<TripRequestDTO> GetTripsAsync(int page, int pageSize);
    Task<bool> DeleteClientAsync(int clientId);
    Task<bool> AssignClientToTripAsync(ClientRequestDTO clientRequest);
}
namespace APBD9.DTOs;

public record TripDTO(
    string Name,
    string Description,
    string DateFrom,
    string DateTo,
    int MaxPeople,
    List<CountryDTO> Countries,
    List<ClientDTO> Clients);
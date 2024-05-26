namespace APBD9.DTOs;

public record ClientRequestDTO(
    string FirstName,
    string LastName,
    string Email,
    string Telephone,
    string Pesel,
    int IdTrip,
    string TripName,
    DateTime? PaymentDate);
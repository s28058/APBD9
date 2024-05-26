namespace APBD9.DTOs;

public record TripRequestDTO(
    int PageNum, 
    int PageSize, 
    int AllPages,
    List<TripDTO> Trips);
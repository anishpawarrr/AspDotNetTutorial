namespace GameStore.Dto;

public record class gameDto(
    int Id, 
    string Name, 
    string Genre, 
    decimal Price, 
    DateOnly ReleaseDate
);

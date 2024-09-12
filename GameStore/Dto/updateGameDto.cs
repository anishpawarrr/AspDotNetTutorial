namespace GameStore.Dto;

public record class updateGameDto(
    string Name, 
    int GenreId, 
    decimal Price, 
    DateOnly ReleaseDate
);

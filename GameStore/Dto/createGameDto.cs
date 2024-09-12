namespace GameStore.Dto;

public record class createGameDto(
    string Name, 
    int GenreId,
    decimal Price, 
    DateOnly ReleaseDate
);

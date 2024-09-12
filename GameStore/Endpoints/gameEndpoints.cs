using GameStore.Data;
using GameStore.Dto;
using GameStore.Entities;
using Microsoft.EntityFrameworkCore;
namespace GameStore.Endpoints;

public static class gameEndpoints
{
    private static readonly List<gameDto> games = [
        new(1, "G1", "g1", 1M, new DateOnly(2003, 1, 1)),
        new(2, "G2", "g2", 2M, new DateOnly(2003, 1, 2)),
        new(3, "G3", "g3", 3M, new DateOnly(2003, 1, 3))
    ];

    public static WebApplication MapGamesEndpoints(this WebApplication app){
        app.MapGet("/", () => "Hello World!");
        
        // app.MapGet("/games", () => games);
        app.MapGet("/games", (GameStoreContext dbContext) => {
            return dbContext.Games.ToListAsync();
        });

        app.MapGet("/games/{id}", (int id, GameStoreContext dbContext) => {
            // int idx = games.FindIndex(games => games.Id == id);
            // return idx<0?Results.Json(data: new {message = "Invalid id"}, statusCode: 500):Results.Json(data: games[idx], statusCode: 200);
            Game? game = dbContext.Games.Find(id);

            if(game is null)return Results.Json(data: new {message = "Invalid id"}, statusCode: 500);

            // Genre? genre = dbContext.Genres.Find(game.GenreId);
            Genre? genre = dbContext.Genres.Find(game.GenreId);

            if(genre is null)return Results.Json(data: new {message = "Invalid genre id"}, statusCode: 500);


            gameDto gameDto = new(
                game.Id,
                game.Name,
                genre.Name,
                game.Price,
                game.ReleaseDate
            );

            return Results.Json(data: gameDto, statusCode: 200);
        });

        app.MapPost("/games", (createGameDto newGame, GameStoreContext dbContext) => {
            // gameDto game = new gameDto(
            //     games.Count+1,
            //     newGame.Name,
            //     newGame.Genre,
            //     newGame.Price,
            //     newGame.ReleaseDate
            // );
            // games.Add(game);

            Game game = new(){
                Name = newGame.Name,
                Genre = dbContext.Genres.Find(newGame.GenreId),
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
 
            dbContext.Add(game);
            dbContext.SaveChanges();

            gameDto gameDto = new (
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            );

            return Results.Json(data: new {status = "success", entry = gameDto}, statusCode: 200);
        });

        app.MapPut("/games/{id}", (int id, updateGameDto updateGame, GameStoreContext dbContext) => {
            // int idx = games.FindIndex(game => game.Id == id);
            // games[idx] = new gameDto(
            //     id,
            //     updateGame.Name,
            //     updateGame.Genre,
            //     updateGame.Price,
            //     updateGame.ReleaseDate
            // );

            Game? game = dbContext.Games.Find(id);

            if(game is null)return Results.NotFound();

            game.Name = updateGame.Name;
            game.GenreId = updateGame.GenreId;
            game.Price = updateGame.Price;
            game.ReleaseDate = updateGame.ReleaseDate;

            dbContext.SaveChanges();

            return Results.Accepted();
        
        });

        app.MapDelete("/games/{id}", (int id, GameStoreContext dbContext) => {
            // int idx = games.FindIndex(game => game.Id == id);
            // if(idx>=0)games.RemoveAt(idx);
            // return Results.Json(data: new {status = "success"}, statusCode: 200);
            
            dbContext.Games.Where(game => game.Id == id).ExecuteDelete();
            return Results.Accepted();
        });

        return app;
    }
}

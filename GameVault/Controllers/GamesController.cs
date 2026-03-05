using GameVault.Models;
using GameVault.Repositories;
using Microsoft.AspNetCore.Mvc;
using GameVault.RequestModels;


namespace GameVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : BaseController
    {
        private readonly IGameRepository _gameRepository;

        public GamesController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        [HttpGet]
        public Task<ActionResult<List<Game>>> GetAllGames()
        {
            return ExecuteListAsync(() => _gameRepository.GetAllGamesAsync());
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<Game?>> GetGameByTitle(string title)
        {
            var error = RequireString(title, "Название игры");
            if (error != null) return error;

            return await ExecuteGetAsync(() => _gameRepository.GetGameByTitleAsync(title));
        }

        [HttpPost]
        public async Task<ActionResult> CreateGame([FromBody] CreateGameRequest request)
        {
            var error1 = RequireString(request.Title, "Название игры");
            if (error1 != null) return error1;

            var error2 = RequireString(request.Company, "Издатель");
            if (error2 != null) return error2;

            var error3 = RequirePositive(request.Weight, "Вес игры");
            if (error3 != null) return error3;

            return await ExecuteAsync(
                () => _gameRepository.CreateGameAsync(request.Title, request.Company, request.Weight, request.ReleaseDate),
                $"Игра '{request.Title}' создана"
            );
        }

        [HttpPut]
        public async Task<ActionResult> UpdateGame([FromBody] UpdateGameRequest request)
        {
            var error1 = RequireString(request.Title, "Название игры");
            if (error1 != null) return error1;

            return await ExecuteAsync(
                () => _gameRepository.UpdateGameAsync(request.Title, request.NewCompany, request.NewWeight, request.NewReleaseDate),
                $"Игра '{request.Title}' обновлена"
            );
        }

        [HttpDelete("{title}")]
        public async Task<ActionResult> DeleteGame(string title)
        {
            var error = RequireString(title, "Название игры");
            if (error != null) return error;

            return await ExecuteAsync(
                () => _gameRepository.DeleteGameAsync(title),
                $"Игра '{title}' удалена"
            );
        }

        [HttpGet("slice/{sliceNumber}")]
        public async Task<ActionResult<List<Game>>> GetGamesSlice(int sliceNumber)
        {
            var error = RequireMin(sliceNumber, 0, "Номер среза");
            if (error != null) return error;

            return await ExecuteListAsync(() => _gameRepository.GetGamesSliceAsync(sliceNumber));
        }

        [HttpGet("slice/by-genre")]
        public async Task<ActionResult<List<Game>>> GetGamesSliceByGenre([FromQuery] int sliceNumber, [FromQuery] string genreName)
        {
            var error1 = RequireMin(sliceNumber, 0, "Номер среза");
            if (error1 != null) return error1;

            var error2 = RequireString(genreName, "Название жанра");
            if (error2 != null) return error2;

            return await ExecuteListAsync(() => _gameRepository.GetGamesSliceByGenreAsync(sliceNumber, genreName));
        }

        [HttpGet("slice/by-publisher")]
        public async Task<ActionResult<List<Game>>> GetGamesSliceByPublisher([FromQuery] int sliceNumber, [FromQuery] string publisherName)
        {
            var error1 = RequireMin(sliceNumber, 0, "Номер среза");
            if (error1 != null) return error1;

            var error2 = RequireString(publisherName, "Название издателя");
            if (error2 != null) return error2;

            return await ExecuteListAsync(() => _gameRepository.GetGamesSliceByPublisherAsync(sliceNumber, publisherName));
        }

        [HttpGet("by-genre/{genreName}")]
        public async Task<ActionResult<List<Game>>> GetGamesByGenre(string genreName)
        {
            var error = RequireString(genreName, "Название жанра");
            if (error != null) return error;

            return await ExecuteListAsync(() => _gameRepository.GetGamesByGenreAsync(genreName));
        }

        [HttpGet("by-publisher/{publisherName}")]
        public async Task<ActionResult<List<Game>>> GetGamesByPublisher(string publisherName)
        {
            var error = RequireString(publisherName, "Название издателя");
            if (error != null) return error;

            return await ExecuteListAsync(() => _gameRepository.GetGamesByPublisherAsync(publisherName));
        }

        [HttpPost("add-genre")]
        public async Task<ActionResult> AddGenreToGame([FromBody] GameGenreRequest request)
        {
            var error1 = RequireString(request.GameName, "Название игры");
            if (error1 != null) return error1;

            var error2 = RequireString(request.GenreName, "Название жанра");
            if (error2 != null) return error2;

            return await ExecuteAsync(
                () => _gameRepository.AddGenreToGameAsync(request.GameName, request.GenreName),
                $"Жанр '{request.GenreName}' добавлен к игре '{request.GameName}'"
            );
        }

        [HttpDelete("remove-genre")]
        public async Task<ActionResult> RemoveGenreFromGame([FromBody] GameGenreRequest request)
        {
            var error1 = RequireString(request.GameName, "Название игры");
            if (error1 != null) return error1;

            var error2 = RequireString(request.GenreName, "Название жанра");
            if (error2 != null) return error2;

            return await ExecuteAsync(
                () => _gameRepository.RemoveGenreFromGameAsync(request.GameName, request.GenreName),
                $"Жанр '{request.GenreName}' удален у игры '{request.GameName}'"
            );
        }

        [HttpGet("{gameName}/genres")]
        public async Task<ActionResult<List<Genre>>> GetGameGenres(string gameName)
        {
            var error = RequireString(gameName, "Название игры");
            if (error != null) return error;

            return await ExecuteListAsync(() => _gameRepository.GetGameGenresAsync(gameName));
        }

        [HttpGet("{gameName}/players")]
        public async Task<ActionResult<List<Player>>> GetPlayersByGame(string gameName)
        {
            var error = RequireString(gameName, "Название игры");
            if (error != null) return error;

            return await ExecuteListAsync(() => _gameRepository.GetPlayersByGameAsync(gameName));
        }
    }
}
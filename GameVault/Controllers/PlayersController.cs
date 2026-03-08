using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameVault.Models;
using GameVault.Repositories;
using Microsoft.AspNetCore.Mvc;
using GameVault.RequestModels;

namespace GameVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : BaseController
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [HttpGet]
        public Task<ActionResult<List<Player>>> GetAllPlayers()
        {
            return ExecuteListAsync(() => _playerRepository.GetAllPlayersAsync());
        }

        [HttpGet("{nickname}")]
        public async Task<ActionResult<Player?>> GetPlayerByNickname(string nickname)
        {
            var error = RequireString(nickname, "Никнейм");
            if (error != null) return error;

            return await ExecuteGetAsync(() => _playerRepository.GetPlayerByNicknameAsync(nickname));
        }

        [HttpPost]
        public async Task<ActionResult> CreatePlayer([FromBody] CreatePlayerRequest request)
        {
            var error1 = RequireString(request.Nickname, "Никнейм");
            if (error1 != null) return error1;

            var error2 = RequireString(request.Email, "Email");
            if (error2 != null) return error2;

            var error3 = RequireMin(request.Level, 0, "Уровень");
            if (error3 != null) return error3;

            if (request.RegistrationDate < new DateTime(2000, 1, 1))
            {
                return BadRequest(new { message = "Дата регистрации не может быть раньше 2000 года" });
            }

            if (request.Level > 999)
            {
                return BadRequest(new { message = "Уровень не может быть выше 999" });
            }

            return await ExecuteAsync(
                () => _playerRepository.CreatePlayerAsync(request.Nickname, request.Email, request.RegistrationDate, request.Level),
                $"Игрок '{request.Nickname}' успешно создан"
            );
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePlayer([FromBody] UpdatePlayerRequest request)
        {
            var error1 = RequireString(request.Nickname, "Никнейм");
            if (error1 != null) return error1;

            if (request.NewLevel > 999)
            {
                return BadRequest(new { message = "Уровень не может быть выше 999" });
            }

            return await ExecuteAsync(
                () => _playerRepository.UpdatePlayerAsync(request.Nickname, request.NewEmail, request.NewLevel),
                $"Игрок '{request.Nickname}' успешно обновлен"
            );
        }

        [HttpDelete("{nickname}")]
        public async Task<ActionResult> DeletePlayer(string nickname)
        {
            var error = RequireString(nickname, "Никнейм");
            if (error != null) return error;

            return await ExecuteAsync(
                () => _playerRepository.DeletePlayerAsync(nickname),
                $"Игрок '{nickname}' успешно удален"
            );
        }

        [HttpGet("slice/{sliceNumber}")]
        public async Task<ActionResult<List<Player>>> GetPlayersSlice(int sliceNumber)
        {
            var error = RequireMin(sliceNumber, 0, "Номер среза");
            if (error != null) return error;

            return await ExecuteListAsync(() => _playerRepository.GetPlayersSliceAsync(sliceNumber));
        }

        [HttpGet("{nickname}/games")]
        public async Task<ActionResult<List<Game>>> GetPlayerGames(string nickname)
        {
            var error = RequireString(nickname, "Никнейм");
            if (error != null) return error;

            return await ExecuteListAsync(() => _playerRepository.GetPlayerGamesAsync(nickname));
        }

        [HttpPost("add-game")]
        public async Task<ActionResult> AddGameToPlayer([FromBody] PlayerGameRequest request)
        {
            var error1 = RequireString(request.Nickname, "Никнейм");
            if (error1 != null) return error1;

            var error2 = RequireString(request.GameTitle, "Название игры");
            if (error2 != null) return error2;

            return await ExecuteAsync(
                () => _playerRepository.AddGameToPlayerAsync(request.Nickname, request.GameTitle),
                $"Игра '{request.GameTitle}' добавлена игроку '{request.Nickname}'"
            );
        }

        [HttpDelete("remove-game")]
        public async Task<ActionResult> RemoveGameFromPlayer([FromBody] PlayerGameRequest request)
        {
            var error1 = RequireString(request.Nickname, "Никнейм");
            if (error1 != null) return error1;

            var error2 = RequireString(request.GameTitle, "Название игры");
            if (error2 != null) return error2;

            return await ExecuteAsync(
                () => _playerRepository.RemoveGameFromPlayerAsync(request.Nickname, request.GameTitle),
                $"Игра '{request.GameTitle}' удалена у игрока '{request.Nickname}'"
            );
        }
    }
}
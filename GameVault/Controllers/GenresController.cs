using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameVault.DTO;
using GameVault.Models;
using GameVault.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : BaseController
    {
        private readonly IGenreRepository _genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        [HttpGet]
        public Task<ActionResult<List<Genre>>> GetAllGenres()
        {
            return ExecuteListAsync(() => _genreRepository.GetAllGenresAsync());
        }

        [HttpPost]
        public async Task<ActionResult> CreateGenre([FromBody] string genreName)
        {
            var error = RequireString(genreName, "Название жанра");
            if (error != null) return error;

            return await ExecuteAsync(
                () => _genreRepository.CreateGenreAsync(genreName),
                $"Жанр '{genreName}' успешно создан"
            );
        }

        [HttpPut]
        public async Task<ActionResult> UpdateGenre([FromBody] UpdateGenreRequest request)
        {
            var error1 = RequireString(request.OldGenreName, "Старое название жанра");
            if (error1 != null) return error1;

            var error2 = RequireString(request.NewGenreName, "Новое название жанра");
            if (error2 != null) return error2;

            return await ExecuteAsync(
                () => _genreRepository.UpdateGenreAsync(request.OldGenreName, request.NewGenreName),
                $"Жанр '{request.OldGenreName}' обновлен на '{request.NewGenreName}'"
            );
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteGenre([FromBody] string genreName)
        {
            var error = RequireString(genreName, "Название жанра");
            if (error != null) return error;

            return await ExecuteAsync(
                () => _genreRepository.DeleteGenreAsync(genreName),
                $"Жанр '{genreName}' удален"
            );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.Controllers
{
    /// <summary>
    /// Абстрактный базовый класс для всех контроллеров API.
    /// Предоставляет унифицированные методы валидации и выполнения запросов.
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Проверяет строковый параметр на пустоту или пробелы.
        /// </summary>
        /// <param name="value">Проверяемая строка.</param>
        /// <param name="parameterName">Название параметра для сообщения об ошибке.</param>
        /// <returns>ActionResult с ошибкой BadRequest если строка пустая, иначе null.</returns>
        /// <remarks>
        /// Используется в начале методов контроллера для валидации входных данных.
        /// 
        /// Пример:
        /// <code>
        /// var error = RequireString(title, "Название игры");
        /// if (error != null) return error;
        /// </code>
        /// </remarks>
        protected ActionResult? RequireString(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BadRequest($"{parameterName} не может быть пустым");
            
            return null;
        }

        /// <summary>
        /// Проверяет числовой параметр на минимальное значение.
        /// </summary>
        /// <typeparam name="T">Тип числа (должен реализовывать IComparable).</typeparam>
        /// <param name="value">Проверяемое число.</param>
        /// <param name="minValue">Минимально допустимое значение.</param>
        /// <param name="parameterName">Название параметра для сообщения об ошибке.</param>
        /// <returns>ActionResult с ошибкой BadRequest если число меньше минимума, иначе null.</returns>
        /// <remarks>
        /// Используется для проверки чисел на неотрицательность или другие минимальные границы.
        /// 
        /// Примеры:
        /// <code>
        /// // Проверка что число >= 0
        /// var error = RequireMin(sliceNumber, 0, "Номер среза");
        /// 
        /// // Проверка что число >= 1
        /// var error = RequireMin(pageNumber, 1, "Номер страницы");
        /// </code>
        /// </remarks>
        protected ActionResult? RequireMin<T>(T value, T minValue, string parameterName) where T : IComparable<T>
        {
            if (value.CompareTo(minValue) < 0)
            {
                string message = $"{parameterName} должен быть не меньше {minValue}";
                return BadRequest(message);
            }
            
            return null;
        }

        /// <summary>
        /// Проверяет числовой параметр на положительное значение (> 0).
        /// </summary>
        /// <typeparam name="T">Тип числа (должен реализовывать IComparable).</typeparam>
        /// <param name="value">Проверяемое число.</param>
        /// <param name="parameterName">Название параметра для сообщения об ошибке.</param>
        /// <returns>ActionResult с ошибкой BadRequest если число ≤ 0, иначе null.</returns>
        /// <remarks>
        /// Используется для проверки веса игр, количества и других величин,
        /// которые должны быть строго положительными.
        /// 
        /// Пример:
        /// <code>
        /// var error = RequirePositive(request.Weight, "Вес игры");
        /// if (error != null) return error;
        /// </code>
        /// </remarks>
        protected ActionResult? RequirePositive<T>(T value, string parameterName) where T : IComparable<T>
        {
            if (value.CompareTo(default!) <= 0)
                return BadRequest($"{parameterName} должен быть положительным числом");
            return null;
        }

        /// <summary>
        /// Выполняет запрос к репозиторию, который возвращает ОДИН ОБЪЕКТ (не список).
        /// Автоматически обрабатывает ошибки и возвращает соответствующие HTTP статусы.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого объекта.</typeparam>
        /// <param name="action">Асинхронная функция, возвращающая объект типа T.</param>
        /// <returns>
        /// - 200 OK с объектом, если объект найден
        /// - 404 Not Found, если объект равен null
        /// - 500 Internal Server Error при ошибке
        /// </returns>
        /// <remarks>
        /// Пример:
        /// <code>
        /// [HttpGet("{nickname}")]
        /// public Task&lt;ActionResult&lt;Player&gt;&gt; GetPlayer(string nickname)
        /// {
        ///     return ExecuteGetAsync(() => _playerRepository.GetPlayerByNicknameAsync(nickname));
        /// }
        /// </code>
        /// </remarks>
        protected async Task<ActionResult<T>> ExecuteGetAsync<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();
                
                if (result == null)
                    return NotFound("Элемент не найден");
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Выполняет запрос к репозиторию, который возвращает СПИСОК ОБЪЕКТОВ.
        /// Автоматически обрабатывает ошибки и возвращает соответствующие HTTP статусы.
        /// </summary>
        /// <typeparam name="T">Тип объектов в списке.</typeparam>
        /// <param name="action">Асинхронная функция, возвращающая список объектов типа T.</param>
        /// <returns>
        /// - 200 OK со списком объектов (может быть пустым)
        /// - 404 Not Found, если список равен null
        /// - 500 Internal Server Error при ошибке
        /// </returns>
        /// <remarks>
        /// Пример:
        /// <code>
        /// [HttpGet]
        /// public Task&lt;ActionResult&lt;List&lt;Game&gt;&gt;&gt; GetAllGames()
        /// {
        ///     return ExecuteListAsync(() => _gameRepository.GetAllGamesAsync());
        /// }
        /// </code>
        /// </remarks>
        protected async Task<ActionResult<List<T>>> ExecuteListAsync<T>(Func<Task<List<T>>> action)
        {
            try
            {
                var result = await action();

                if (result == null)
                    return NotFound("Элементы не найдены");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Выполняет операцию в репозитории, которая НЕ ВОЗВРАЩАЕТ ДАННЫЕ (CREATE, UPDATE, DELETE).
        /// Автоматически обрабатывает ошибки и возвращает соответствующие HTTP статусы.
        /// </summary>
        /// <param name="action">Асинхронная функция, выполняющая операцию.</param>
        /// <param name="successMessage">Сообщение об успешном выполнении.</param>
        /// <returns>
        /// - 200 OK с сообщением об успехе
        /// - 500 Internal Server Error при ошибке
        /// </returns>
        /// <remarks>
        /// Пример:
        /// <code>
        /// [HttpPost]
        /// public async Task&lt;ActionResult&gt; CreateGame([FromBody] CreateGameRequest request)
        /// {
        ///     return await ExecuteAsync(
        ///         () => _gameRepository.CreateGameAsync(...),
        ///         $"Игра '{request.Title}' создана"
        ///     );
        /// }
        /// </code>
        /// </remarks>
        protected async Task<ActionResult> ExecuteAsync(Func<Task> action, string successMessage)
        {
            try
            {
                await action();
                return Ok(successMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }
}
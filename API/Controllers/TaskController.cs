using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TaskController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        /// <summary>
        /// Создает задачу и возвращает GUID задачи
        /// </summary>
        /// <response code="202">GUID созданной задачи</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> Post()
        {
            string newGuid = await _tasksService.CreateTaskAsync();

            Response.OnCompleted(async () =>
            {
                await _tasksService.ExecuteTaskAsync(newGuid);
            });

            return Accepted((object)newGuid);
        }

        /// <summary>
        /// Возвращает статус запрошенной задачи
        /// </summary>
        /// <param name="guid">GUID задачи</param>
        /// <response code="200">Статус и время последнего обновления</response>
        /// <response code="400">Передан не GUID</response>
        /// <response code="404">Задачи с переданным GUID не существует</response>
        [HttpGet("{guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SomeTaskModel>> Get(string guid)
        {
            var status = await _tasksService.GetTaskStatusAsync(guid);
            return Ok(status);
        }
    }
}

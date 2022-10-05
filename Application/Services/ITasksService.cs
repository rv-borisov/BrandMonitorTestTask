using Application.Models;

namespace Application.Services
{
    public interface ITasksService
    {
        Task<string> CreateTaskAsync();

        Task<SomeTaskModel> GetTaskStatusAsync(Guid guid);
        Task<SomeTaskModel> GetTaskStatusAsync(string guid);

        Task ExecuteTaskAsync(string guid);
    }
}
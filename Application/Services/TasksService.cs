using Application.Models;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    internal class TasksService : ITasksService
    {
        private readonly IDbContext _db;

        public TasksService(IDbContext db)
        {
            _db = db;
        }

        public async Task<string> CreateTaskAsync()
        {
            var newTask = new SomeTask
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                Status = SomeTask.Statuses.Created
            };

            await _db.SomeTasks.AddAsync(newTask);
            await _db.SaveChangesAsync();

            return newTask.Id.ToString();
        }

        public async Task<SomeTaskModel> GetTaskStatusAsync(string guid)
        {
            if (Guid.TryParse(guid, out Guid result))
            {
                return await GetTaskStatusAsync(result);
            }

            throw new WrongDataFormatException("Wrong data format! Expected GUID.");
        }

        public async Task<SomeTaskModel> GetTaskStatusAsync(Guid guid)
        {
            var task = await _db.SomeTasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == guid);

            if (task is null)
            {
                throw new NotFoundException("Not found Task.");
            }

            return new SomeTaskModel
            {
                Status = task.GetStatusName(),
                Timestamp = task.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            };
        }

        public async Task ExecuteTaskAsync(string guid)
        {
            var task = await _db.SomeTasks.FirstOrDefaultAsync(t => t.Id == new Guid(guid));
            if (task is not null)
            {
                task.Running();
                await _db.SaveChangesAsync();

                Thread.Sleep(120000);

                task.Finish();
                await _db.SaveChangesAsync();
            }
        }
    }
}

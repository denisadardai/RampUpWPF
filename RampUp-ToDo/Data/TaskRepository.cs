using DynamicData;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Interfaces;

namespace RampUp_ToDo.Data
{
    public class TaskRepository : ITaskRepository
    {
        private DatabaseService<TaskModel> _dbService;

        public TaskRepository(DatabaseService<TaskModel> dbService)
        {
            _dbService = dbService;
        }

        public TaskRepository()
        {
        }

        public IEnumerable<TaskModel> GetAll()
        {
            var tasks = _dbService.GetAllTasks();
            return tasks;
        }

        public TaskModel Add(TaskModel task)
        {
            _dbService.Insert(task);
            return task;
        }

        public Task<TaskModel> Update(TaskModel task) => throw new NotImplementedException();

        public Task<TaskModel> Delete(TaskModel task) => throw new NotImplementedException();

        public Task<bool> SaveAll() => throw new NotImplementedException();
    }
}

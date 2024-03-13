using RampUp_ToDo.Entities;

namespace RampUp_ToDo.Interfaces
{
    public interface ITaskRepository
    {
        IEnumerable<TaskModel> GetAll();
        TaskModel Add(TaskModel task);

        Task<TaskModel> Update(TaskModel task);
        Task<TaskModel> Delete(TaskModel task);
        Task<bool> SaveAll();
    }
}

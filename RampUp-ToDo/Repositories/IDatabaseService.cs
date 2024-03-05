using RampUp_ToDo.Entities;
using System.Collections.ObjectModel;

namespace RampUp_ToDo.Repositories
{
    public interface IDatabaseService<TEntity>
        where TEntity : TaskModel, new()
    {
        ObservableCollection<TaskModel> Tasks { get; }
        bool Insert(TEntity entity);
        bool Delete(TEntity entity);
        void Update(TEntity entity);

        IEnumerable<TEntity> GetAll()
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                var toDo = new TaskModel
                {
                    Id = Tasks[i].Id,
                    Status = Tasks[i].Status,
                    Description = Tasks[i].Description,
                    AssignedTo = Tasks[i].AssignedTo
                };
                yield return (TEntity)toDo;
            }
        }

    }
}

using DynamicData;
using DynamicData.Binding;
using Microsoft.EntityFrameworkCore;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Models;
using RampUp_ToDo.ViewModels;

namespace RampUp_ToDo.Data
{
    public abstract class DatabaseService<TEntity> : DbContext
    {
        public static SourceList<TaskModel> Tasks = new();
        public IObservableCollection<TaskModel> TasksList = (IObservableCollection<TaskModel>)Tasks.Connect();
        public IEnumerable<TagModel> Tags { get; set; }
        public abstract IEnumerable<StateModel> States { get; set; }
        public abstract bool Insert(TEntity entity);
        public abstract bool Delete(TEntity entity);
        public abstract void Update(TEntity entity);

        public virtual IEnumerable<TaskModel> GetAllTasks()
        {

            foreach (var task in TasksList)
            {
                var toDo = new TaskModel
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    AssignedTo = task.AssignedTo,
                    State = task.State,
                    TagsList = task.TagsList
                };
                yield return toDo;
            }

        }

        public virtual IEnumerable<StateModel> GetAllStates()
        {
            foreach (var state in States)
            {
                var s = new StateModel
                {
                    Id = state.Id,
                    Name = state.Name
                };
                yield return s;
            }

        }
        public virtual IEnumerable<TagModel> GetAllTags()
        {
            foreach (var tag in Tags)
            {
                var s = new TagModel
                {
                    Id = tag.Id,
                    Name = tag.Name
                };
                yield return s;
            }

        }

        public abstract IObservableCollection<TaskModel> GetFilteredData(FilterViewModel filterVM);
        public abstract IObservableCollection<TaskModel> Search(string name);
    }
}

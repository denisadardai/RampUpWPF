using RampUp_ToDo.Entities;
using RampUp_ToDo.Models;

namespace RampUp_ToDo.Data
{
    public static class DatabaseFactory{
        public static DatabaseService<TaskModel> GetDataContext(StoringType storingType)
        {
            DatabaseService<TaskModel> store = storingType switch
            {
                StoringType.JSON => DataContextFile.Instance,
                StoringType.DB => DataContextDB.Instance,
                _ => throw new ArgumentOutOfRangeException(nameof(storingType), storingType, null)
            };
            return store;
        }
    }
    public abstract class DatabaseService<TEntity>
    {
        public IEnumerable<TaskModel> Tasks { get; set; }
        public IEnumerable<TagModel> Tags { get; set; }
        public abstract bool Insert(TEntity entity);
        public abstract bool Delete(TEntity entity);
        public abstract void Update(TEntity entity);
        public abstract IEnumerable<TaskModel> GetAllTasks();
        public abstract IEnumerable<TagModel> GetAllTags();
        public abstract IEnumerable<TaskModel> Search(string name);
        public abstract void AddTag(TagModel newtag);
    }
}

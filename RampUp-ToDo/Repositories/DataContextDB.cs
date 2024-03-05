using LiteDB;
using RampUp_ToDo.Entities;
using System.Collections.ObjectModel;

namespace RampUp_ToDo.Repositories
{
    public class DataContextDB : IDatabaseService<TaskModel>
    {
        public LiteDatabase DB { get; set; }
        public LiteCollection<TaskModel> Col { get; set; }

        public ObservableCollection<TaskModel> Tasks{ get; set; }

        public DataContextDB()
        {
            DB = new LiteDatabase(@"C:\Users\denisa.dardai\source\repos\RampUp-ToDo\data.db");
            Col = (LiteCollection<TaskModel>?)DB.GetCollection<TaskModel>("tasks");
            var tasklist = Col.FindAll().ToList();
            Tasks = new ObservableCollection<TaskModel>(tasklist);
        }
        public bool Insert(TaskModel entity)
        {
            Col.Insert(entity);
            return (true);
        }

        public bool Delete(TaskModel entity)
        {
            var exists = Col.FindOne(t => t.Id == entity.Id);
            if (exists != null)
            {
                Col.Delete(entity.Id);
            }

            return true;
        }

        public void Update(TaskModel entity)
        {
            Col.Update(entity);
        }
        public IEnumerable<TaskModel> GetAll()
        {
            return Tasks;
        }
    }
}

using Newtonsoft.Json;
using RampUp_ToDo.Entities;
using System.Collections.ObjectModel;
using System.IO;

namespace RampUp_ToDo.Repositories
{
    public class DataContextFile : IDatabaseService<TaskModel>
    {
        readonly string _path;
        public ObservableCollection<TaskModel> Tasks { get; set; }

        public DataContextFile()
        {
            _path = @"C:\Users\denisa.dardai\source\repos\RampUp-ToDo\data.json";
            LoadJsonFromDisk();
        }
        public void LoadJsonFromDisk()
        {
            if (File.Exists(_path).Equals(true))
            {
                var list = JsonConvert.DeserializeObject<ObservableCollection<TaskModel>>(File.ReadAllText(_path));

                if (list != null)
                {
                    Tasks = list;
                }
                else
                {
                    Tasks = [];
                }
            }
            else
            {
                Tasks = [];
            }
        }

        public bool Insert(TaskModel entity)
        {
            Tasks.Add(entity);
            var json = JsonConvert.SerializeObject(Tasks);
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }

            File.WriteAllText(_path, json);
            return (true);
        }

        public bool Delete(TaskModel entity)
        {
            if (Tasks== null)
            {
                Tasks = [];
            }

            var exists = Tasks.FirstOrDefault(t => t.Id == entity.Id);
            if (exists != null)
            {
                Tasks.Remove(entity);
                Insert(entity);
            }

            return true;
        }

        public void Update(TaskModel entity)
        {
            Insert(entity);
        }

        public IEnumerable<TaskModel> GetAll()
        {
            return Tasks;
        }
    }
}

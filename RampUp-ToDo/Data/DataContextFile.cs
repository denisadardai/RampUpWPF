using Newtonsoft.Json;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace RampUp_ToDo.Data
{
    public class DataContextFile : DatabaseService<TaskModel>
    {
        private static DataContextFile _instance;
        public static DataContextFile Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataContextFile();

                return _instance;
            }
        }
        readonly string _path;

        public DataContextFile()
        {
            _path = @"C:\Users\denisa.dardai\source\repos\denisadardai\RampUpWPF\dataFile.json";
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

        public override bool Insert(TaskModel entity)
        {
            Tasks.ToList().Add(entity);
            var json = JsonConvert.SerializeObject(Tasks);
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }

            File.WriteAllText(_path, json);
            return true;
        }

        public override bool Delete(TaskModel entity)
        {
            if (Tasks == null)
            {
                Tasks = [];
            }

            var exists = Tasks.FirstOrDefault(t => t.Id == entity.Id);
            if (exists != null)
            {
                Tasks.ToList().Remove(entity);
                var json = JsonConvert.SerializeObject(Tasks);
                if (File.Exists(_path))
                {
                    File.Delete(_path);
                }

                File.WriteAllText(_path, json);
            }

            return true;
        }

        public override void Update(TaskModel entity)
        {
            Insert(entity);
        }


        public override IEnumerable<TaskModel> GetAllTasks()
        {

            foreach (var task in Tasks)
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
        public override IEnumerable<TaskModel> Search(string name)
        {
            Tasks = Tasks.Where(t => t.Name.Contains(name));
            return Tasks;
        }

        public override void AddTag(TagModel newtag)
        {
            var task = Tasks.FirstOrDefault(x => x.Id == newtag.TaskId);
            task.TagsList.Add(newtag);
            Insert(task);
        }

        public override IEnumerable<TagModel> GetAllTags()
        {
            Tags = [];
            return Tags;
        }
    }
}

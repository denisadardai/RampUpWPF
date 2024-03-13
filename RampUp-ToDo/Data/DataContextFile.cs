using DynamicData;
using DynamicData.Binding;
using Newtonsoft.Json;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Models;
using RampUp_ToDo.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace RampUp_ToDo.Data
{
    public class DataContextFile : DatabaseService<TaskModel>
    {
        readonly string _path;
        public static SourceList<TaskModel> Tasks = new();
        public IObservableCollection<TaskModel> TasksList = new ObservableCollectionExtended<TaskModel>();
        public override IEnumerable<StateModel> States { get; set; }
        // public IEnumerable<TagModel> Tags { get; set; }

        public DataContextFile()
        {
            _path = @"C:\Users\denisa.dardai\source\repos\denisadardai\RampUpWPF\data.json";
            Tasks.Connect()
                .ObserveOn(Scheduler.CurrentThread)
                .Bind(TasksList)
                .Subscribe();
            LoadJsonFromDisk();
        }
        public void LoadJsonFromDisk()
        {
            if (File.Exists(_path).Equals(true))
            {
                var list = JsonConvert.DeserializeObject<IObservableCollection<TaskModel>>(File.ReadAllText(_path));

                if (list != null)
                {
                    TasksList = list;
                }
                else
                {
                    TasksList = new ObservableCollectionExtended<TaskModel>();
                }
            }
            else
            {
                TasksList = new ObservableCollectionExtended<TaskModel>();
            }
        }

        public override bool Insert(TaskModel entity)
        {
            TasksList.Add(entity);
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
            if (TasksList == null)
            {
                TasksList = new ObservableCollectionExtended<TaskModel>();
            }

            var exists = TasksList.FirstOrDefault(t => t.Id == entity.Id);
            if (exists != null)
            {
                TasksList.Remove(entity);
                var json = JsonConvert.SerializeObject(TasksList);
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
            return TasksList;
        }
        public override IEnumerable<StateModel> GetAllStates()
        {
            return States;
        }

        public override IObservableCollection<TaskModel> GetFilteredData(FilterViewModel filterVM) => throw new NotImplementedException();
        public override IObservableCollection<TaskModel> Search(string name) => throw new NotImplementedException();
    }
}

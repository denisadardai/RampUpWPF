using DynamicData;
using DynamicData.Binding;
using LiteDB;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Models;
using RampUp_ToDo.ViewModels;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace RampUp_ToDo.Data
{
    public class DataContextDB : DatabaseService<TaskModel>
    {
        public LiteDatabase DB { get; set; }
        public LiteCollection<TaskModel> ColTasks { get; set; }
        public LiteCollection<TagModel> ColTags { get; set; }
        public LiteCollection<StateModel> ColStates { get; set; }
        public override IEnumerable<StateModel> States { get; set; }

        public static SourceList<TaskModel> Tasks = new();
        public IObservableCollection<TaskModel> TasksList = new ObservableCollectionExtended<TaskModel>();
        // public ReadOnlyObservableCollection<TaskModel> filteredDataCollection;

        public DataContextDB()
        {
            //filteredDataCollection = new ReadOnlyObservableCollection<TaskModel>(TasksList);
            DB = new LiteDatabase(@"C:\Users\denisa.dardai\source\repos\denisadardai\RampUpWPF\data.db");
            ColTasks = (LiteCollection<TaskModel>?)DB.GetCollection<TaskModel>("tasks");
            var tasklist = ColTasks.FindAll().ToList();
            TasksList = new ObservableCollectionExtended<TaskModel>();

            ColTags = (LiteCollection<TagModel>?)DB.GetCollection<TagModel>("tags");
            var tagsList = ColTags.FindAll().ToList();
            Tags = new ObservableCollection<TagModel>(tagsList);

            ColStates = (LiteCollection<StateModel>?)DB.GetCollection<StateModel>("states");
            var stateList = ColStates.FindAll().ToList();
            States = new ObservableCollection<StateModel>(stateList);

            Tasks.Connect()
                .ObserveOn(Scheduler.CurrentThread)
                .Bind(TasksList)
                .Subscribe();
            InitStates();

        }

        private void InitStates()
        {
            if (States.ToArray().Length == 0)
            {
                var pro1 = new StateModel
                {
                    Name = "New"
                };
                ColStates.Insert(pro1);
                var pro2 = new StateModel
                {
                    Name = "In Progress"
                };
                ColStates.Insert(pro2);
                var pro3 = new StateModel
                {
                    Name = "Done"
                };
                ColStates.Insert(pro3);
            }
        }

        public override bool Insert(TaskModel entity)
        {
            var tags = entity.TagsList;
            foreach (var tag in tags)
            {
                ColTags.Insert(tag);
            }

            ColTasks.Insert(entity);
            return true;
        }

        public override bool Delete(TaskModel entity)
        {
            var exists = ColTasks.FindOne(t => t.Id == entity.Id);
            if (exists != null)
            {
                ColTasks.Delete(entity.Id);
            }

            return true;
        }

        public override void Update(TaskModel entity)
        {
            ColTasks.Update(entity);
        }
        public ObservableCollection<TaskModel> GetFilteredByTags(IEnumerable<TagModel> tags)
        {
            ObservableCollection<TaskModel> all = [];
            foreach (var tag in tags)
            {
                all = (ObservableCollection<TaskModel>)ColTasks.FindAll()
                    .Where(t => t.TagsList.Contains(tag));
            }
            return all;
        }

        public ObservableCollection<TaskModel> GetFilteredByProgress(IEnumerable<StateModel> progresses)
        {
            ObservableCollection<TaskModel> all = [];
            foreach (var progress in progresses)
            {
                all = (ObservableCollection<TaskModel>)ColTasks.FindAll()
                    .Where(t => t.State.Name == progress.Name);
            }
            return all;
        }

        public override IObservableCollection<TaskModel> GetFilteredData(FilterViewModel filterVM)
        {
            TasksList = (IObservableCollection<TaskModel>)Tasks.Connect().Filter(FilterCriteria)
                .ObserveOn(Scheduler.CurrentThread);
            //   .Bind(out filteredDataCollection).Subscribe();
            return TasksList;
        }

        private bool FilterCriteria(TaskModel model) => throw new NotImplementedException();

        public override IObservableCollection<TaskModel> Search(string name)
        {
            TasksList = (IObservableCollection<TaskModel>)ColTasks.FindAll().Where(t => t.Name.Contains(name));
            return TasksList;
        }
    }
}
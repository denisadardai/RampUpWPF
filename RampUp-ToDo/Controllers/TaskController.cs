using DynamicData;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Models;
using RampUp_ToDo.ViewModels;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using RampUp_ToDo.Data;
using RampUp_ToDo.Interfaces;

namespace RampUp_ToDo.Controllers
{
    public class TaskController
    {
        private static TaskController _instance;
        public static TaskController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TaskController();
                return _instance;
            }
        }

        private ITaskRepository _taskRepository;

        private static SourceList<TaskModel> _tasks = new();
        public IObservableList<TaskModel> TasksList => _tasks;

        public IEnumerable<StateModel> States { get; set; }

        public IEnumerable<TagModel> Tags { get; set; }

        private DatabaseService<TaskModel> _db;
        private TagRepository _tagRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            //_taskSubject = new Subject<TaskModel>();
            _taskRepository = taskRepository;
            //States = _db.GetAllStates();
            Tags = _db.GetAllTags();

        }

        public TaskController()
        {
        }

        public IEnumerable<TaskModel> GetAllTasks()
        {
            _tasks = _taskRepository.GetAll();
            return TasksList;
        }
        public IEnumerable<TagModel> GetAllTags()
        {
            return _tagRepository.GetAll();
        }

        //public IEnumerable<StateModel> GetAllStates()
        // {
        //     return _db.GetAllStates();
        // }
        public void AddTask(TaskModel task)
        {
            _taskRepository.Add(task);
            TasksList.Add(task);
            Tasks.Connect()
                .ObserveOn(Scheduler.CurrentThread)
                .Bind(TasksList)
                .Subscribe();
        }

        public IEnumerable<TaskModel> GetSearchData(string searchContent)
        {
            TasksList = _db.Search(searchContent);
            return TasksList;
        }

        public FilterViewModel GetAllFilters()
        {
            FilterViewModel filters = new FilterViewModel(States, Tags);
            return filters;
        }
    }
}

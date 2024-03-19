using DynamicData;
using RampUp_ToDo.Data;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Models;
using RampUp_ToDo.ViewModels;

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
        private static SourceList<TaskModel> _tasks = new();

        public IObservableList<TaskModel> TasksList => _tasks;

        public TaskController()
        {
            //States = GetAllStates();
        }
        public void FillData(StoringType storage)
        {
            var store = DatabaseFactory.GetDataContext(storage);
            _tasks.Clear();
            _tasks.AddRange(store.GetAllTasks());
        }

        public void AddTask(string name, string description, string assignedTo, string tag, StoringType storage)
        {

            var store = DatabaseFactory.GetDataContext(storage);
            TaskModel newtask = new TaskModel
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                AssignedTo = assignedTo,
                StoringType = storage
            };
            TagModel newTag = new TagModel
            {
                Name = tag,
                TaskId = newtask.Id,
                Id = Guid.NewGuid()
            };
            newtask.TagsList = [newTag];
            newtask.State = StateTypes.New;
            store.Insert(newtask);
            _tasks.Add(newtask);
            _tasks.Clear();
            _tasks.AddRange(store.GetAllTasks());
        }

        public void Delete(TaskModel obj, StoringType storage)
        {
            var store = DatabaseFactory.GetDataContext(storage);
            _tasks.Remove(obj);
            store.Delete(obj);
            _tasks.Clear();
            _tasks.AddRange(store.GetAllTasks());
            //FillData(storage);
        }
        public IEnumerable<TagModel> GetAllTags(StoringType storage)
        {
            var store = DatabaseFactory.GetDataContext(storage);
            var tags = store.GetAllTags();
            //_tags.Clear();
            //_tags.AddRange(store.GetAllTags());
            return tags;
        }

        public void UpdateTaskState(TaskModel taskModel, StateViewModel stateSelected, StoringType storage)
        {
            var store = DatabaseFactory.GetDataContext(storage);
            var model = _tasks.Items.SingleOrDefault(X => X.Id == taskModel.Id);
            model.State = stateSelected.StateType;
            store.Update(model);
            _tasks.Clear();
            _tasks.AddRange(store.GetAllTasks());
        }


        public void Search(string name, StoringType storage)
        {
            var store = DatabaseFactory.GetDataContext(storage);
            var tasks= store.Search(name);
            _tasks.Clear();
            _tasks.AddRange(tasks);
        }

        public void AddTag(string tag, TaskModel obj, StoringType storage)
        {
            var store = DatabaseFactory.GetDataContext(storage);
            TagModel newtag = new TagModel
            {
                Name = tag,
                Id = Guid.NewGuid(),
                TaskId = obj.Id
            };
            store.AddTag(newtag);
            _tasks.Clear();
            _tasks.AddRange(store.GetAllTasks());
        }
    }
}

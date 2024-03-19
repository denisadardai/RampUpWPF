using LiteDB;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Models;
using RampUp_ToDo.ViewModels;
using System.Collections.ObjectModel;

namespace RampUp_ToDo.Data
{
    public class DataContextDB : DatabaseService<TaskModel>
    {
        private static DataContextDB _instance;
        public static DataContextDB Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataContextDB();

                return _instance;
            }
        }
        public LiteDatabase DB { get; set; }
        public LiteCollection<TaskModel> ColTasks { get; set; }
        public LiteCollection<TagModel> ColTags { get; set; }
        public LiteCollection<StateViewModel> ColStates { get; set; }

        public DataContextDB()
        {
            DB = new LiteDatabase(@"C:\Users\denisa.dardai\source\repos\denisadardai\RampUpWPF\dataLite.db");
            ColTasks = (LiteCollection<TaskModel>?)DB.GetCollection<TaskModel>("tasks").Include(t => t.TagsList)
                .Include(s => s.State);
            var tasklist = ColTasks.FindAll().ToList();
            Tasks = new ObservableCollection<TaskModel>(tasklist);

            ColTags = (LiteCollection<TagModel>?)DB.GetCollection<TagModel>("tags");
            var tagsList = ColTags.FindAll().ToList();
            Tags = new ObservableCollection<TagModel>(tagsList);

        }

        public override IEnumerable<TaskModel> GetAllTasks()
        {

            foreach (var task in Tasks)
            {
                var toDo =new TaskModel
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    AssignedTo = task.AssignedTo,
                    State = task.State,

                };
                toDo.TagsList = ColTags.FindAll().Where(t => t.TaskId == toDo.Id).ToList();
                yield return toDo;
            }

        }
        public override IEnumerable<TagModel> GetAllTags()
        {
            foreach (var tag in Tags)
            {
                var s = new TagModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    TaskId = tag.TaskId
                };
                yield return s;
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
                foreach (var tag in entity.TagsList)
                {
                    ColTags.Delete(tag.Id);
                }
                ColTasks.Delete(entity.Id);
            }

            return true;
        }

        public override void Update(TaskModel entity)
        {
            ColTasks.Update(entity);
        }


        public override IEnumerable<TaskModel> Search(string name)
        {
            var tasks = ColTasks.FindAll().Where(t => t.Name.Contains(name));
            return tasks;
        }

        public override void AddTag(TagModel newtag)
        {
            ColTags.Insert(newtag);
        }

    }
}
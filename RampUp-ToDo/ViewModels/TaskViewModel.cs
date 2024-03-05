using RampUp_ToDo.Commands;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RampUp_ToDo.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private readonly DataContextFile _frepo;
        private readonly DataContextDB _dbrepo;
        public event PropertyChangedEventHandler? PropertyChanged;
        private bool _status;
        private string _selectedStorageType;
        private ObservableCollection<TaskModel> _allToDos;

        public string[] StoringTypes { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public TaskViewModel()
        {
            _frepo = new DataContextFile();
            _dbrepo = new DataContextDB();
            AllTodos = [];
            StoringTypes = ["JSON", "LiteDB"];
            AddCommand = new RelayCommand(AddTask, CanAddTask);
            DeleteCommand = new RelayCommand<TaskModel>(DeleteTask);
            CheckedCommand = new RelayCommand<TaskModel>(CheckTask);
        }

        public ObservableCollection<TaskModel> FillData(string type)
        {
            if (type != null)
            {
                if (type == "JSON")
                {
                    AllTodos = new ObservableCollection<TaskModel>(_frepo.GetAll());
                }
                else if (type == "LiteDB")
                {
                    AllTodos = new ObservableCollection<TaskModel>(_dbrepo.GetAll());
                }
            }

            return AllTodos;
        }

        private void AddTask(object obj)
        {
            TaskModel newtask = new TaskModel();
            for (int i = 0; i < AllTodos.Count; i++)
            {
                if (i == AllTodos.Count - 1)
                {
                    var taskId = AllTodos[i].Id;
                    newtask.Id = taskId + 1;
                }
            }
            newtask.Status = false;
            newtask.Description = Description;
            newtask.AssignedTo = AssignedTo;
            AllTodos.Add(newtask);
            if (SelectedStorageType == "JSON")
            {
                _frepo.Insert(newtask);
            }
            else
            {
                _dbrepo.Insert(newtask);
            }

        }
        private bool CanAddTask(object obj)
        {
            if (Description !=null && AssignedTo != null)
            {
                return true;
            }
            return false;
        }
        private void DeleteTask(TaskModel obj)
        {
            AllTodos.Remove(obj);
            if (SelectedStorageType == "JSON")
            {
                _frepo.Delete(obj);
            }
            else
            {
                _dbrepo.Delete(obj);
            }
        }
        private void CheckTask(TaskModel model)
        {
            model.Status = true;
            for (int i = 0; i < AllTodos.Count; i++)
            {
                if (AllTodos[i].Id == model.Id)
                {
                    AllTodos[i].Status = model.Status;
                }
            }

            if (SelectedStorageType == "JSON")
            {
                _frepo.Update(model);
            }
            else
            {
                _dbrepo.Update(model);
            }
        }
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public ObservableCollection<TaskModel> AllTodos{
            get => _allToDos;
            set
            {
                _allToDos = value;
                OnPropertyChanged(nameof(AllTodos));
            }}
       public string Description { get; set; }
       public string AssignedTo { get; set;}
       public string SelectedStorageType
       {
           get => _selectedStorageType;
           set
           {
               if (_selectedStorageType != value)
               {
                   _selectedStorageType = value;
                    OnPropertyChanged(nameof(SelectedStorageType));
                    AllTodos = FillData(_selectedStorageType);
               }
           }
       }
        public bool Status
       {
           get => _status;
           set
           {
               if (_status != value)
               {
                   _status = value;

                   OnPropertyChanged(nameof(Status));
               }
           }
       }
    }
}

using DynamicData;
using DynamicData.Binding;
using RampUp_ToDo.Commands;
using RampUp_ToDo.Controllers;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Models;
using RampUp_ToDo.Utils;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace RampUp_ToDo.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private StoringType _selectedStorageType;

        private IEnumerable<StateViewModel> _stateChanged;
        private string _tag;

       // private StateViewModel _stateSelected = DefaultState();
        private readonly IDisposable _taskDisposable;
        private ObservableCollectionExtended<TaskViewModel> _allToDos;
        private IEnumerable<TagViewModel>? _tagsChanged;
        private BehaviorSubject<Func<TaskModel, bool>> _filter = new(x => true);
        private StateViewModel _stateSelected;
        private string _ntag;

        public string Tag
        {
            get => _tag;
            set
            {
                _tag = value;
                OnPropertyChanged(nameof(Tag));
            }
        }
        public string NewTag
        {
            get => _ntag;
            set
            {
                _ntag = value;
                OnPropertyChanged(nameof(NewTag));
            }
        }
        public IEnumerable<StateViewModel>? States
        {
            get => _stateChanged;
            set
            {
                if (_stateChanged != value)
                {
                    _stateChanged = value;
                }
                OnPropertyChanged(nameof(States));
            }
        }
        public IEnumerable<TagViewModel>? Tags
        {
            get => _tagsChanged;
            set
            {
                if (_tagsChanged != value)
                {
                    _tagsChanged = value;
                }
                OnPropertyChanged(nameof(Tags));
            }
        }
        public IEnumerable<StoringType> StoringTypes { get; } = Enum.GetValues(typeof(StoringType)).Cast<StoringType>();
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand StateCommand { get; set; }
     //   public ICommand CheckedCommand { get; set; }
        public ICommand TagCommand { get; set; }
      //  public ICommand ApplyFiltersCommand { get; set; }

       //public TaskModel SelectedTask { get; set; }
        public ICommand SearchCommand { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public string Name { get; set; }
        public ObservableCollectionExtended<TaskViewModel> AllToDos
        {
            get => _allToDos;
            set
            {
                if (_allToDos != value)
                {
                    _allToDos = value;
                }
                OnPropertyChanged(nameof(AllToDos));
            }
        }
        public MainViewModel()
        {

            AllToDos = [];
            States = GetAllStates();
            Tags = GetAllTags();
            _taskDisposable = TaskController.Instance.TasksList.Connect()
                .ObserveOn(Scheduler.CurrentThread)
                .Filter(_filter)
                .Transform(x=>new TaskViewModel(x,States))
                .Bind(AllToDos)
                .Subscribe();
            AddCommand = new RelayCommand(AddTask, CanAddTask);
            DeleteCommand = new RelayCommand<TaskModel>(DeleteTask);
            TagCommand = new RelayCommand<TaskModel>(AddTag);
            SearchCommand = new RelayCommand(SearchTask, CanSearchTask);
            StateCommand = new RelayCommand<TaskModel>(UpdateTaskState);
        }

        private void AddTag(TaskModel obj)
        {
            TaskController.Instance.AddTag(NewTag, obj, SelectedStorageType);
        }

        public IEnumerable<StateViewModel>? GetAllStates()
        {
            var stateViewModels = new List<StateViewModel>();
            var s1 = new StateViewModel(SetFilters) { Checked = false, Id = 1, Name = "New", StateType = StateTypes.New };
            var s2 = new StateViewModel(SetFilters) { Checked = false, Id = 2, Name = "InProgress", StateType = StateTypes.InProgress };
            var s3 = new StateViewModel(SetFilters) { Checked = false, Id = 3, Name = "Done", StateType = StateTypes.Done };
            stateViewModels.Add(s1);
            stateViewModels.Add(s2);
            stateViewModels.Add(s3);
            return stateViewModels;
        }
        public IEnumerable<TagViewModel>? GetAllTags()
        {
            var tagViewModels = new List<TagViewModel>();
            var tags = TaskController.Instance.GetAllTags(SelectedStorageType).ToList();
            for (var i = 0; i < tags.Count; i++)
            {
                var t1 = new TagViewModel(SetFilters, tags[i])
                {
                    Checked = false,
                    Tag = { Id = tags[i].Id, Name = tags[i].Name, TaskId = tags[i].TaskId },
                    Name = tags[i].Name,
                    Id = i
                };
                tagViewModels.Add(t1);
            }
            return tagViewModels;
        }
        public void FillData()
        {
            TaskController.Instance.FillData(SelectedStorageType);
            Tags = GetAllTags();
        }
        private void AddTask(object _)
        {
            TaskController.Instance.AddTask(Name,Description,AssignedTo,Tag,SelectedStorageType);
        }
        private bool CanAddTask(object _)
        {
            if (Description != null && AssignedTo != null && Tag != null)
            {
                return true;
            }
            return false;
        }
        private void DeleteTask(TaskModel model)
        {
            TaskController.Instance.Delete(model, SelectedStorageType);
        }

        private void UpdateTaskState(TaskModel model)
        {
            TaskController.Instance.UpdateTaskState(model, StateSelected, SelectedStorageType);
        }

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private bool CanSearchTask(object _)
        {
            if (Name != "")
            {
                return true;
            }
            return false;
        }

        private void SearchTask(object _)
        {
           TaskController.Instance.Search(Name, SelectedStorageType);
        }

        public StoringType SelectedStorageType
        {
            get => _selectedStorageType;
            set
            {
                if (_selectedStorageType != value)
                {
                    _selectedStorageType = value;
                    OnPropertyChanged(nameof(SelectedStorageType));
                    FillData();
                }
            }
        }

        private void SetFilters()
        {
            Predicate<TaskModel> predicateState = task => States.Any(x=>x.Checked && x.StateType == task.State);
            Predicate<TaskModel> predicateTag = task => Tags.Any(x => x.Checked && task.TagsList.Any(tag=>tag.Id == x.Tag.Id));
            var combinedPredicates = PredicateExtension.AND(predicateState, predicateTag);
            _filter.OnNext(combinedPredicates);
        }

        public StateViewModel StateSelected
        {
            get => _stateSelected;
            set
            {
                _stateSelected = value;
                OnPropertyChanged(nameof(StateSelected));
                //UpdateTaskState();
            }
        }
        public void Dispose()
        {
            _taskDisposable.Dispose();
        }
    }
}

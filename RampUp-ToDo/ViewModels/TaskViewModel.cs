using DynamicData;
using DynamicData.Binding;
using RampUp_ToDo.Commands;
using RampUp_ToDo.Controllers;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Interfaces;
using RampUp_ToDo.Models;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;

namespace RampUp_ToDo.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private StorageViewModel _selectedStorageType;
        private FilterViewModel _selectedFilters;
        private StateModel _stateSelected;
        private string _searchContent;
        private ITaskRepository _taskRepository;
        private readonly object _taskDisposable;

        public TagModel Tag { get; set; }
        public IEnumerable<StorageViewModel> StoringTypes { get; set; }
        public FilterViewModel Filters { get; set; }
        public IEnumerable<StateModel> StateTypes { get; set; }
        public IEnumerable<TagModel> TagsTypes { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        //public ICommand CheckedCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public TaskViewModel(IEnumerable<StorageViewModel> storageTypes)
        {

            AllToDos = new ObservableCollectionExtended<TaskModel>();
            // Convert SourceList to ObservableCollection
            _taskDisposable = TaskController.Instance.TasksList.Connect()
                .ObserveOn(Scheduler.CurrentThread)
                .Bind(AllToDos)
                .Subscribe();
            // Filters = filters;
            Tag = new TagModel();
            StoringTypes = storageTypes;
            // StateTypes = _taskController.GetAllStates();
            //TagsTypes = _taskController.GetAllTags();
            AddCommand = new RelayCommand(AddTask, CanAddTask);
            DeleteCommand = new RelayCommand<TaskModel>(DeleteTask);
            SearchCommand = new RelayCommand(SearchTask, CanSearchTask);
        }

        public TaskViewModel(TaskController taskController)
        {


            Filters = TaskController.GetAllFilters();

        }


        private bool CanSearchTask(object obj)
        {
            if (Name != "")
            {
                return true;
            }
            return false;
        }

        private void SearchTask(object obj)
        {
            AllToDos = new ObservableCollectionExtended<TaskModel>(SelectedStorageType.Storage.Search(SearchContent));
        }

        public void FillData(string type)
        {
            if (type != null)
            {
                foreach (var stype in StoringTypes)
                {
                    if (type == stype.Name)
                    {
                        AllToDos = new ObservableCollectionExtended<TaskModel>(stype.Storage.GetAllTasks());
                    }
                }
            }
        }

        private void OnTasksChanged(IChangeSet<TaskModel> set)
        {
            foreach (var change in set)
            {
                //var item = change.Range.Count > 0 : change.Item.Current;

            }
        }

        private void AddTask(object obj)
        {

            TaskModel newtask = new TaskModel
            {
                Id = new Guid(),
                Name = Name,
                Description = Description,
                AssignedTo = AssignedTo
            };
            Tag.TaskId = newtask.Id;
            Tag.Id = new Guid();
            newtask.TagsList.Append(Tag);
            newtask.State = new StateModel()
            {
                Id = StateTypes.ToArray()[0].Id,
                Name = StateTypes.ToArray()[0].Name
            };
            foreach (var stype in StoringTypes)
            {
                if (stype.Name == stype.Name)
                {
                    stype.Storage.Insert(newtask);
                }
            }
            _taskController.AddTask(newtask);

        }
        private bool CanAddTask(object obj)
        {
            if (Description != null && AssignedTo != null)
            {
                return true;
            }
            return false;
        }
        private void DeleteTask(TaskModel obj)
        {
            AllToDos.Remove(obj);
            foreach (var stype in StoringTypes)
            {
                if (SelectedStorageType.Name == stype.Name)
                {
                    stype.Storage.Delete(obj);
                }
            }
        }

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public ObservableCollectionExtended<TaskModel> AllToDos { get; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public string Name { get; set; }

        public string SearchContent
        {
            get => _searchContent;
            set
            {
                if (_searchContent != value)
                {
                    _searchContent = value;
                    OnPropertyChanged(nameof(SearchContent));
                    //AllToDos = _taskController.GetSearchData(SearchContent);
                }
            }
        }
        public StorageViewModel SelectedStorageType
        {
            get => _selectedStorageType;
            set
            {
                if (_selectedStorageType != value)
                {
                    _selectedStorageType = value;
                    OnPropertyChanged(nameof(SelectedStorageType));
                    // AllToDos = FillData(SelectedStorageType.Name);
                }
            }
        }
        public FilterViewModel SelectedFilter
        {
            get => _selectedFilters;
            set
            {
                if (_selectedFilters != value)
                {
                    _selectedFilters = value;
                    OnPropertyChanged(nameof(SelectedFilter));
                    SetFilters(_selectedFilters);
                    // AllToDos = FilteredData(SelectedFilter);
                }
            }
        }

        private void SetFilters(FilterViewModel selectedFilter)
        {
            throw new NotImplementedException();
        }

        public StateModel StateSelected
        {
            get => _stateSelected;
            set
            {
                if (_stateSelected != value)
                {
                    _stateSelected = value;
                    OnPropertyChanged(nameof(StateSelected));
                    //  AllToDos = FilteredData(SelectedFilter);
                }
            }
        }

        private ObservableCollectionExtended<TaskModel> FilteredData(FilterViewModel filterVM)
        {
            if (filterVM != null)
            {
                AllToDos = new ObservableCollectionExtended<TaskModel>(SelectedStorageType.Storage.GetFilteredData(filterVM));
            }
            return (ObservableCollectionExtended<TaskModel>)AllToDos;
        }

        public void Dispose()
        {
            taskDisposable.Dispose();
        }
    }
}

using RampUp_ToDo.Entities;
using System.ComponentModel;

namespace RampUp_ToDo.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        private TaskModel _taskChanged;
        public IEnumerable<StateViewModel> _states;

        private StateViewModel _stateSelected;
        public TaskModel Task
        {
            get => _taskChanged;
            set
            {
                _taskChanged = value;
                OnPropertyChanged(nameof(Task));
            }
        }
        public IEnumerable<StateViewModel>? States
        {
            get => _states;
            set
            {
                if (_states != value)
                {
                    _states = value;
                }
                OnPropertyChanged(nameof(States));
            }
        }
        public StateViewModel StateSelected
        {
            get => _stateSelected;
            set
            {
                if (_stateSelected != value)
                {
                    _stateSelected = value;
                }
                OnPropertyChanged(nameof(StateSelected));
            }
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public TaskViewModel(TaskModel task, IEnumerable<StateViewModel> states)
        {
            Task = task;
            States = states;
            StateSelected = States.FirstOrDefault(x => x.StateType == Task.State);
        }



    }
}

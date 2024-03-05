using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RampUp_ToDo.Entities
{
    public class TaskModel : INotifyPropertyChanged
    {
        private string? _description;
        private bool _status;
        private string? _assignedTo;
        public int Id { get; set; }
        public string? Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
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

        public string? AssignedTo
        {
            get => _assignedTo;
            set
            {
                if (_assignedTo != value)
                {
                    _assignedTo = value;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

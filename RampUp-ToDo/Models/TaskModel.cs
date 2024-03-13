using RampUp_ToDo.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RampUp_ToDo.Entities
{
    public class TaskModel : INotifyPropertyChanged
    {
        private string? _description;
        private string? _assignedTo;
        public Guid Id { get; set; }
        public string Name { get; set; }
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
        public IEnumerable<TagModel> TagsList { get; set; }
        public StateModel State { get; set; }

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

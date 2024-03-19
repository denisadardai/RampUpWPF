using RampUp_ToDo.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RampUp_ToDo.ViewModels
{
    public class TagViewModel :INotifyPropertyChanged
    {
        private readonly Action _onChecked;
        private bool _checked;

        public int Id { get; set; }
        public string Name { get; set; }
        public TagModel Tag { get; set; }
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                OnPropertyChanged(nameof(Checked));
                _onChecked();
            }
        }

        public TagViewModel(Action onChecked, TagModel tag)
        {
            _onChecked = onChecked;
            Tag = tag;
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

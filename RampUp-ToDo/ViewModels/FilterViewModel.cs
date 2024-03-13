using RampUp_ToDo.Commands;
using RampUp_ToDo.Controllers;
using RampUp_ToDo.Models;
using System.Windows.Input;

namespace RampUp_ToDo.ViewModels
{
    public class FilterViewModel
    {
        private readonly TaskController _taskController;
        public IEnumerable<StateModel> States { get; set; }

        public IEnumerable<TagModel> Tags { get; set; }
        public FilterViewModel SelectedFilters { get; set; }
        public IEnumerable<StateModel> SelectedStates { get; set; }
        public ICommand CheckedStateCommand { get; set; }
        public ICommand CheckedTagCommand { get; set; }
        public FilterViewModel(IEnumerable<StateModel> states, IEnumerable<TagModel> tags)
        {
            States = states;
            Tags = tags;
            CheckedStateCommand = new RelayCommand<StateModel>(CheckState);
            CheckedTagCommand = new RelayCommand<TagModel>(CheckTag);
        }

        private void CheckTag(TagModel model) => throw new NotImplementedException();
        private void CheckState(StateModel model) => throw new NotImplementedException();

        public FilterViewModel(TaskController taskController)
        {

            _taskController = taskController;
            States = _taskController.GetAllStates();
            Tags = _taskController.GetAllTags();

        }

    }
}

using RampUp_ToDo.Models;
namespace RampUp_ToDo.Entities
{
    public class TaskModel
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
        public IList<TagModel> TagsList { get; set; }
        public StateTypes State { get; set; }
        public StoringType StoringType { get; set; }
    }
}

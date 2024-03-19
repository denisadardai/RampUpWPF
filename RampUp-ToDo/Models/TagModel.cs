namespace RampUp_ToDo.Models
{
    public class TagModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TaskId { get; set; }
    }
}

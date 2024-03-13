using RampUp_ToDo.Data;
using RampUp_ToDo.Entities;

namespace RampUp_ToDo.ViewModels
{
    public class StorageViewModel()
    {
        public string Name { get; set; }
        public DatabaseService<TaskModel> Storage { get; set; }

        public StorageViewModel(string name, DatabaseService<TaskModel> storageType) : this()
        {
            Name = name;
            Storage = storageType;
        }
    }
}

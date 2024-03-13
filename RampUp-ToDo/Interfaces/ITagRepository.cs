using DynamicData.Binding;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Models;

namespace RampUp_ToDo.Interfaces
{
    public interface ITagRepository
    {
        IObservableCollection<TagModel> GetAll();
        Task<TagModel> Add(TaskModel tag);

        Task<TagModel> Update(TagModel tag);
        Task<TagModel> Delete(TagModel tag);
        Task<bool> SaveAll();
    }
}

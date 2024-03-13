using DynamicData.Binding;
using RampUp_ToDo.Entities;
using RampUp_ToDo.Interfaces;
using RampUp_ToDo.Models;

namespace RampUp_ToDo.Data
{
    public class TagRepository : ITagRepository
    {
        private DatabaseService<TagModel> _dbService;

        public TagRepository(DatabaseService<TagModel> dbService)
        {
            _dbService = dbService;
        }

        public IObservableCollection<TagModel> GetAll()
        {
            var tags = _dbService.GetAllTags();
            return (IObservableCollection<TagModel>)tags;
        }
        public Task<TagModel> Add(TaskModel tag) => throw new NotImplementedException();
        public Task<TagModel> Update(TagModel task) => throw new NotImplementedException();

        public Task<TagModel> Delete(TagModel task) => throw new NotImplementedException();

        public Task<bool> SaveAll() => throw new NotImplementedException();
    }
}

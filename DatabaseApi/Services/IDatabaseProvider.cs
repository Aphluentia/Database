namespace DatabaseApi.Services
{
    public interface IDatabaseProvider<T>
    {
        public Task<List<T>> FindAllAsync();
        public Task<T?> FindByIdAsync(string id);
        public Task CreateAsync(T newObject);
        public Task UpdateAsync(string id, T updatedObject);
        public Task RemoveByIdAsync(string id);
        public Task RemoveAllAsync();
    }
}

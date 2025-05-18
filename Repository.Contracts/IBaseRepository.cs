namespace Repository.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        void Update(T entity);
        Task Create(T entity);
        void Delete(T entity);
        Task Save();
    }
}

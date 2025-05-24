namespace Service.Contracts
{
    public interface IAddressService<T> where T : class
    {
        Task<T> SetAddressAsync(T obj, Guid id);
    }
}

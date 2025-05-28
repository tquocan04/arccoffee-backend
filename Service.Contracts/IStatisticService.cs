namespace Service.Contracts
{
    public interface IStatisticService
    {
        Task<Dictionary<string, decimal?>> GetRevenueCategoriesByYearMonthAsync(int year, int? month = null);
        Task<Dictionary<string, decimal?>> GetRevenueByCategoriyInYearMonthAsync(Guid categoryId,
            int year, int? month = null);
    }
}

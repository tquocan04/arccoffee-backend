using DTOs;
using Entities;
using ExceptionHandler.Statistic;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class StatisticService(IMapper mapper,
        IBillRepository billRepository,
        IProductRepository productRepository,
        ICategoryRepository categoryRepository) : IStatisticService
    {
        private async Task<List<BillDTO>> MapListOrderBill(List<Order> orders)
        {
            List<BillDTO> list = [];

            for (int i = 0; i < orders.Count; i++)
            {
                list.Add(mapper.Map<BillDTO>(orders[i]));
                if (list[i].Items != null)
                    await MapProductToItemDTO(list[i]);
            }
            return list;
        }

        private async Task MapProductToItemDTO(BillDTO billDTO)
        {
            if (billDTO.Items != null && billDTO.Items.Count > 0)
            {
                foreach (var item in billDTO.Items)
                {
                    var product = await productRepository.GetProductByIdAsync(item.ProductId);

                    if (product != null)
                        mapper.Map(product, item);
                }
            }
        }

        private async Task<Dictionary<string, decimal?>> GetRevenueAsync(List<BillDTO> list, Guid? categoryId = null)
        {
            Dictionary<string, decimal?> revenueByCategory = [];

            foreach (var order in list)
            {
                if (order.Items != null && order.Items.Count > 0)
                {
                    foreach (var item in order.Items)
                    {
                        Product? product = await productRepository.GetProductByIdAsync(item.ProductId);

                        if (product != null)
                        {
                            Category? category = await categoryRepository.GetCategoryByIdAsync(product.CategoryId, false);

                            if (category != null)
                            {
                                if (categoryId == null)
                                    Revenue(item, revenueByCategory, category.Name);
                                else if (product.CategoryId == categoryId)
                                    Revenue(item, revenueByCategory, product.Name);
                            }
                        }

                    }
                }
            }

            if (revenueByCategory.Count == 0)
                throw new NotFoundStatisticException();

            return revenueByCategory;
        }

        private static void Revenue(ItemDTO item, Dictionary<string, decimal?> revenueByCategory, string name)
        {
            decimal? itemRevenue = item.Quantity * item.Price;

            if (revenueByCategory.ContainsKey(name))
            {
                revenueByCategory[name] += itemRevenue;
            }
            else
            {
                revenueByCategory[name] = itemRevenue;
            }
        }

        public async Task<Dictionary<string, decimal?>> GetRevenueCategoriesByYearMonthAsync(int year, int? month = null)
        {
            var bills = (month == null) ? await billRepository.GetCompletedBillByYearMonthAsync(year)
                : await billRepository.GetCompletedBillByYearMonthAsync(year, month);

            List<BillDTO> list = await MapListOrderBill(bills);

            return await GetRevenueAsync(list);
        }

        public async Task<Dictionary<string, decimal?>> GetRevenueByCategoriyInYearMonthAsync(Guid categoryId,
            int year, int? month = null)

        {
            var bills = (month == null) ? await billRepository.GetCompletedBillByYearMonthAsync(year)
                : await billRepository.GetCompletedBillByYearMonthAsync(year, month);

            List<BillDTO> list = await MapListOrderBill(bills);

            return await GetRevenueAsync(list, categoryId);
        }
    }
}

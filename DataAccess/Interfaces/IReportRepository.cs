using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IReportRepository
    {
        Task<List<CustomerPurchaseSummary>> GetCustomerPurchaseSummaryAsync();
    }
}

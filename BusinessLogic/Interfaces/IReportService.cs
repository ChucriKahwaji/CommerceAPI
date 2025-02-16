using BusinessLogic.DTos;

namespace BusinessLogic.Interfaces
{
    public interface IReportService
    {
        Task<List<CustomerPurchaseSummaryDto>> GetCustomerPurchaseSummaryAsync();
    }
}

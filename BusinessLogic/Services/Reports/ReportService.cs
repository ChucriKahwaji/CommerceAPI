using BusinessLogic.DTos;
using BusinessLogic.Interfaces;
using DataAccess.Interfaces;

namespace BusinessLogic.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<CustomerPurchaseSummaryDto>> GetCustomerPurchaseSummaryAsync()
        {
            var summaries = await _reportRepository.GetCustomerPurchaseSummaryAsync();

            // Convert DAL Model to DTO
            return summaries.Select(s => new CustomerPurchaseSummaryDto
            {
                CustomerId = s.CustomerId,
                CustomerName = s.CustomerName,
                TotalOrders = s.TotalOrders,
                TotalSpent = s.TotalSpent
            }).ToList();
        }
    }
}

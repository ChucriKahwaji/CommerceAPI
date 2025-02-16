using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/reports")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("customer-purchases")]
    public async Task<IActionResult> GetCustomerPurchaseSummary()
    {
        var report = await _reportService.GetCustomerPurchaseSummaryAsync();
        return Ok(report);
    }
}

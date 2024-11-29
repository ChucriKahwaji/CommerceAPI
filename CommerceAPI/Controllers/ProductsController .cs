using log4net;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ProductsController));

        [HttpGet(Name = "GetProducts")]
        public IActionResult GetProducts()
        {
            Logger.Info("GET /api/products called");
            return Ok(new { Message = "Products fetched successfully." });
        }
    }
}

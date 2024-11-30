using API.Exceptions;
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
        public IActionResult GetProducts(int id)
        {
            Logger.Info("GET /api/products called");

            if (id <= 0)
                throw new ValidationException("The product ID must be greater than zero.");

            if (id == 999)
                throw new UnauthorizedException("You are not authorized to view this product.");

            if (id == 888)
                throw new ForbiddenException("Access to this product is forbidden.");

            if (id != 1) // Assume only product ID 1 exists for this example
                throw new NotFoundException($"Product with ID {id} was not found.");

            return Ok(new { Message = "Products fetched successfully." });
        }
    }
}

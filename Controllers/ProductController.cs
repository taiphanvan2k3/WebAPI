using LearnApiWeb.Models;
using LearnApiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LearnApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        // Không điền tham số vào route vì đây là các giá trị không required
        public async Task<ActionResult<List<ProductModel>>> FilterProductByName(string name, double? fromPrice, double? toPrice, string sortBy, int page = 1)
        {
            return Ok(await _productRepository.FilterProducts(name, fromPrice, toPrice, sortBy, page));
        }
    }
}
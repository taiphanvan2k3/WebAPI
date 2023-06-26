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

        [HttpGet("{name}")]
        public async Task<ActionResult<List<ProductModel>>> FilterProductByName(string name)
        {
            return Ok(await _productRepository.FilterProducts(name));
        }
    }
}
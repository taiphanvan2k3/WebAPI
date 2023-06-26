using LearnApiWeb.Data;
using LearnApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnApiWeb.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BookStoreContext _context;

        public ProductRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<List<ProductModel>> FilterProducts(string searchName)
        {
            // return await UseLeftJoin_c1(searchName);
            return await UseLeftJoin_c2(searchName);
        }

        public async Task<List<ProductModel>> UseLeftJoin_c1(string searchName)
        {
            var list = await _context.Products
                        .Where(p => p.Name.Contains(searchName))
                        .GroupJoin(
                            _context.Categories,
                            p => p.CategoryId,
                            c => c.Id,
                            (p, c) => new { Product = p, Categories = c }
                        )
                        .SelectMany(
                            i1 => i1.Categories.DefaultIfEmpty(),
                            (i1, i2) => new ProductModel
                            {
                                Id = i1.Product.Id,
                                Name = i1.Product.Name,
                                Price = i1.Product.Price,
                                CategoryName = i2 != null ? i2.Name : null
                            }
                        )
                        .ToListAsync();
            return list;
        }

        public async Task<List<ProductModel>> UseLeftJoin_c2(string searchName)
        {
            // Có thể thực hiện LeftJoin như sau:
            var list = await _context.Products
                            .Where(p => p.Name.Contains(searchName))
                            .Select(p => new
                            {
                                Product = p,
                                Category = _context.Categories.FirstOrDefault(c => c.Id == p.CategoryId)
                            })
                            .Select(x => new ProductModel
                            {
                                Id = x.Product.Id,
                                Name = x.Product.Name,
                                Price = x.Product.Price,
                                CategoryName = x.Category != null ? x.Category.Name : null
                            })
                            .ToListAsync();
            return list;
        }
    }
}
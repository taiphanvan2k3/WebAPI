using LearnApiWeb.Data;
using LearnApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnApiWeb.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BookStoreContext _context;
        public static int PAGE_SIZE { get; set; } = 4;

        public ProductRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<List<ProductModel>> FilterProducts(string searchName, double? fromPrice, double? toPrice, string sortBy, int page = 1)
        {
            return await UseLeftJoin_c1(searchName, fromPrice, toPrice, sortBy, page);
        }

        // Good Query
        public async Task<List<ProductModel>> UseLeftJoin_c1(string searchName, double? fromPrice, double? toPrice, string sortBy, int page = 1)
        {
            // Do AsQueryable() nên mới tạo ra câu truy vấn chứ chưa tạo ra kết quả
            var products = _context.Products.AsQueryable();
            #region Filter
            if (searchName != null)
            {
                products = products.Where(p => p.Name.Contains(searchName));
            }

            if (fromPrice.HasValue)
            {
                products = products.Where(p => p.Price >= fromPrice.Value);
            }

            if (toPrice.HasValue)
            {
                products = products.Where(p => p.Price <= toPrice.Value);
            }
            #endregion

            #region Sorting
            products = products.OrderBy(p => p.Name);
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "tenhh_desc":
                        products = products.OrderByDescending(p => p.Name);
                        break;
                    case "gia_asc":
                        products = products.OrderBy(p => p.Price);
                        break;
                    case "gia_desc":
                        products = products.OrderByDescending(p => p.Price);
                        break;
                }
            }
            #endregion

            // Quy trình Filter -> Sort -> Paging:
            // Do đó ta phải làm theo trình tự query như vậy
            products = SplitPageHelper<Product>.SplitPage(products, page, PAGE_SIZE);

            // Lưu ý rằng chỉ ToList() sau khi đã tạo xong câu truy vấn
            // Vì một khi ToList() rồi thì không thể truy cập đến các navigation property nữa
            return await products.GroupJoin(
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

            /*
            Câu truy vấn của cách này. Sau khi lấy được kết quả paging rồi mới đi left join
            SELECT [t].[Id], [t].[Name], [t].[Price], [c].[Name] AS [CategoryName]
            FROM (
                SELECT [p].[Id], [p].[CategoryId], [p].[Name], [p].[Price]
                FROM [Products] AS [p]
                ORDER BY [p].[Name]
                OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY
            ) AS [t]
            LEFT JOIN [Categories] AS [c] ON [t].[CategoryId] = [c].[Id]
            ORDER BY [t].[Name] 
             */
        }

        //Bad query 
        private async Task<List<ProductModel>> BadQuery(string searchName, double? fromPrice, double? toPrice, string sortBy, int page = 1)
        {
            var products = _context.Products.AsQueryable();
            #region Filter
            if (searchName != null)
            {
                products = products.Where(p => p.Name.Contains(searchName));
            }

            if (fromPrice.HasValue)
            {
                products = products.Where(p => p.Price >= fromPrice.Value);
            }

            if (toPrice.HasValue)
            {
                products = products.Where(p => p.Price <= toPrice.Value);
            }
            #endregion

            #region Sorting
            products = products.OrderBy(p => p.Name);
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "tenhh_desc":
                        products = products.OrderByDescending(p => p.Name);
                        break;
                    case "gia_asc":
                        products = products.OrderBy(p => p.Price);
                        break;
                    case "gia_desc":
                        products = products.OrderByDescending(p => p.Price);
                        break;
                }
            }
            #endregion
            var result = products
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
            );

            // Độ phức tạp của cách này sẽ không tốt bằng
            // Vì câu truy vấn của nó có dạng:
            /*
            SELECT [p].[Id], [p].[Name], [p].[Price], [c].[Name] AS [CategoryName]
            FROM [Products] AS [p]
            LEFT JOIN [Categories] AS [c] ON [p].[CategoryId] = [c].[Id]
            ORDER BY [p].[Name]
            OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY
            Nó phải đi GetAll rồi mới fetch ra thì performance không tốt
            */
            return await SplitPageHelper<ProductModel>.SplitPage(result, page, PAGE_SIZE).ToListAsync();
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStoreSPA.Models;

namespace SportsStoreSPA.Controllers
{
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly DataContext context;

        public ProductController(DataContext ctx)
        {
            context = ctx;
        }

        [HttpGet("{id}")]
        public Product GetProduct(long id)
        {
            Product result = context.Products
                                .Include(p => p.Supplier).ThenInclude(s => s.Products)
                                .Include(p => p.Ratings)
                                .First(p => p.ProductId == id);

            if(result != null)
            {
                if(result.Supplier != null)
                {
                    result.Supplier.Products = result.Supplier.Products.Select(p =>
                    {
                        return new Product
                        {
                            ProductId = p.ProductId,
                            Name = p.Name,
                            Category = p.Category,
                            Description = p.Description,
                            Price = p.Price
                        };
                    });
                }

                if (result.Ratings != null)
                {
                    foreach (Rating r in result.Ratings)
                    {
                        r.Product = null;
                    }
                }
            }

            //System.Threading.Thread.Sleep(5000);
            return result;
        }

        [HttpGet]
        public IEnumerable<Product> GetProducts(bool related = false)
        {
            IQueryable<Product> query = context.Products;

            if (related)
            {
                query = query.Include(p => p.Supplier).Include(p => p.Ratings);
                List<Product> data = query.ToList();
                data.ForEach(p =>
                {
                    if (p.Supplier != null)
                    {
                        p.Supplier.Products = null;
                    }

                    if (p.Ratings != null)
                    {
                        p.Ratings.ForEach(r => r.Product = null);
                    }
                });

                return data;
            } else
            {
                return query;
            }
        }

    }
}

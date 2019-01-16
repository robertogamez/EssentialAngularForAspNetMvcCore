using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStoreSPA.Dtos;
using SportsStoreSPA.Models;
using SportsStoreSPA.Models.BindingTargets;

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

            if (result != null)
            {
                if (result.Supplier != null)
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

        [HttpPost]
        public DataTablesResponse<List<Product>> GetProducts([FromBody]DataTablesOptions options, bool related = false)
        {
            IQueryable<Product> query = context.Products;
            List<Product> products;

            if (!string.IsNullOrWhiteSpace(options.search.value))
            {
                string searchLower = options.search.value.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchLower)
                            || p.Description.ToLower().Contains(searchLower));
            }

            if (related)
            {
                query = query.Include(p => p.Supplier).Include(p => p.Ratings);
                products = query.ToList();

                products.ForEach(p =>
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
            }
            else
            {
                products = query.ToList();
            }


            return new DataTablesResponse<List<Product>>
            {
                data = products.Skip(options.Start).Take(options.Length).ToList(),
                recordsTotal = products.Count,
                recordsFiltered = products.Count
            };
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody]ProductData pdata)
        {
            if (ModelState.IsValid)
            {
                Product p = pdata.Product;

                context.Add(p);
                context.SaveChanges();

                return Ok(p.ProductId);
            } else
            {
                return BadRequest(ModelState);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            return context.Products.Find(id);
        }
    }
}

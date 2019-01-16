using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStoreSPA.Models;
using SportsStoreSPA.Models.BindingTargets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsStoreSPA.Controllers
{
    [Route("api/[controller]")]
    public class SupplierController : Controller
    {
        private readonly DataContext context;

        public SupplierController(DataContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IEnumerable<Supplier> GetSuppliers()
        {
            return context.Suppliers;
        }

        [HttpPost]
        public IActionResult CreateSupplier([FromBody]SupplierData supplierd)
        {
            if (ModelState.IsValid)
            {
                Supplier s = supplierd.Supplier;
                context.Add(s);
                context.SaveChanges();

                return Ok(s.SupplierId);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}

using GeneralStoreApII.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreApII.Controllers
{
    public class ProductController : ApiController
    {
        private readonly ApplicationDbContext _dbContext = ApplicationDbContext.Create();

        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] Products product)
        {
            if (product is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }
            if (ModelState.IsValid)
            {
                _dbContext.Products.Add(product);
                int changeCount = await _dbContext.SaveChangesAsync();

                return Ok("Product has been created");
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Products> product = await _dbContext.Products.ToListAsync();
            return Ok(product);
        }

        [HttpGet]
        
        public async Task<IHttpActionResult> GetBySku([FromUri] string Sku)
        {
            Products products = await _dbContext.Products.FindAsync(Sku);

            if (products != null)
            {
                return Ok(products);
            }

            return NotFound();
        }
    }
}

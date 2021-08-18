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
    public class CustomerController : ApiController
    {
        private readonly ApplicationDbContext _dbContext = ApplicationDbContext.Create();

        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] Customer customer)
        {
            if (customer is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }
            if (ModelState.IsValid)
            {
                _dbContext.Customers.Add(customer);
                int changeCount = await _dbContext.SaveChangesAsync();

                return Ok("Customer has been created");
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Customer> customers = await _dbContext.Customers.ToListAsync();
            return Ok(customers);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Customer customer = await _dbContext.Customers.FindAsync(id);

            if(customer != null)
            {
                return Ok(customer);
            }

            return NotFound();
        }

        [HttpPut]

        public async Task<IHttpActionResult> Put([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest();
            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);

            var existing = await _dbContext.Customers.FindAsync(customer.Id);

            if (existing == null)
                return NotFound();

            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;

            if (await _dbContext.SaveChangesAsync() > 0)
                return Ok("The customer was updated");

            return InternalServerError();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete([FromUri] int id)
        {
            Customer customer = await _dbContext.Customers.FindAsync(id);
            if (customer is null)
                return NotFound();

            _dbContext.Customers.Remove(customer);

            if (await _dbContext.SaveChangesAsync() == 1)
            {
                return Ok("The customer was deleted");
            }

            return InternalServerError();

        }
    }
}

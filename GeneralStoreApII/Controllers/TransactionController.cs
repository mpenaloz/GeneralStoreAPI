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
    public class TransactionController : ApiController
    {
        private readonly ApplicationDbContext _dbContext = ApplicationDbContext.Create();

        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] Transactions transactions)
        {
            if (transactions is null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _dbContext.Transactions.Add(transactions);
                int changeCount = await _dbContext.SaveChangesAsync();

                return Ok("Transaction has been created");
            }

            var customer = await _dbContext.Customers.FindAsync(transactions.CustomerId);
            var product = await _dbContext.Products.FindAsync(transactions.Sku);

            var ValidationResult = this.ValidateTransaction(transactions, product, customer);

            if (!string.IsNullOrWhiteSpace(ValidationResult))
                return BadRequest(ValidationResult);

            product.NumberInInventory = transactions.ItemCount - transactions.ItemCount;

            if (await _dbContext.SaveChangesAsync() > 0)
                return Ok(transactions);

            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Transactions> transactions = await _dbContext.Transactions.ToListAsync();
            return Ok(transactions);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Transactions transactions = await _dbContext.Transactions.FindAsync(id);

            if (transactions != null)
            {
                return Ok(transactions);
            }

            return NotFound();

        }

        private string ValidateTransaction(Transactions transactions, Products product, Customer customer)
        {
            if (customer == null)
                return $"A customer with Id: {transactions.CustomerId} does not exist.";
            if (product == null)
                return $"A Product with the Sku: {transactions.Sku} does not exist.";
            if (product.IsInStock == false)
                return $"A product with the Sku: {transactions.Sku} is not in stock.";
            if (product.NumberInInventory < transactions.ItemCount)
                return $"There is not enough of the product with the Sku of: {transactions.Sku} to complete the transaction.";

            return string.Empty;
        }
    }
}

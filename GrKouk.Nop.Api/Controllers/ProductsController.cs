using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.Nop.Api.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrKouk.Nop.Api.Models;
using Newtonsoft.Json;

namespace GrKouk.Nop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApiDbHandmadeContext _handmadeContext;

        public ProductsController(ApiDbHandmadeContext handmadeContext)
        {
            _handmadeContext = handmadeContext;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _handmadeContext.Product.ToListAsync();
        }
        [HttpGet("ProductCodes")]
        public async Task<ActionResult<IEnumerable<CodeDto>>> GetProductCodes(string codeBase)
        {


            List<CodeDto> items;
            if (string.IsNullOrEmpty(codeBase))
            {
                items = await _handmadeContext.Product.OrderByDescending(p => p.Sku)
                    .Select(t => new CodeDto
                    {
                        Code = t.Sku
                    }).ToListAsync();
            }
            else
            {
                items = await _handmadeContext.Product.Where(p => p.Sku.Contains(codeBase))
                    .OrderByDescending(p => p.Sku)
                    .Select(t => new CodeDto
                    {
                        Code = t.Sku
                    }).ToListAsync();
            }

            return Ok(items);
        }
        [HttpGet("Codes")]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetProductsByCode(string codeBase)
        {
            var items = _handmadeContext.Product
                .Select(p => new ProductListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Sku
                });

            if (!String.IsNullOrEmpty(codeBase))
            {
                items = items.Where(p => p.Code.Contains(codeBase));
            }
            var listItems = await items.OrderByDescending(p => p.Code).ToListAsync();

            return Ok(listItems);
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _handmadeContext.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpGet("ProductAttributeMappings")]
        public async Task<ActionResult<IEnumerable<ProductProductAttributeMapping>>> GetProductAttrMappings(int productId)
        {
            var productAttrMappings = await _handmadeContext.ProductProductAttributeMapping
                //.Include(p=>p.Product)
                //.Include(p=>p.ProductAttributeValue)
                .Where(p => p.ProductId == productId)
                .ToListAsync();
            if (productAttrMappings == null)
            {
                return Ok();
            }
            return Ok(productAttrMappings);
        }
        [HttpGet("ProductAttributeValues")]
        public async Task<ActionResult<IEnumerable<ProductAttributeValue>>> GetProductAttrValues(int productId)
        {
            var productAttrMappings = await _handmadeContext.ProductProductAttributeMapping
               .Where(p => p.ProductId == productId)
                .ToListAsync();
            if (productAttrMappings == null)
            {
                return Ok();
            }

            List<ProductAttributeValue> productAttrValueList = new List<ProductAttributeValue>();

            foreach (var prodAttrMapping in productAttrMappings)
            {
                var prAttrValue = await _handmadeContext.ProductAttributeValue
                    .Where(p => p.ProductAttributeMappingId == prodAttrMapping.Id)
                    .ToListAsync();
                foreach (var productAttributeValue in prAttrValue)
                {
                    productAttrValueList.Add(productAttributeValue);
                }
            }


            return Ok(productAttrValueList);
        }
        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _handmadeContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _handmadeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _handmadeContext.Product.Add(product);
            await _handmadeContext.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _handmadeContext.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _handmadeContext.Product.Remove(product);
            await _handmadeContext.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _handmadeContext.Product.Any(e => e.Id == id);
        }
    }
}

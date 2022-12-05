using APIIngame.Services;
using DataAccessLayer.Concrete;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace APIIngame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private Context _context;
        public ProductController(Context context)
        {
            _context = context;
        }

        //api/product
        [HttpGet]
        public List<Product> Get()
        {
            return _context.Products.Include(x => x.Category).ToList();


        }
        //api/GetCategoryList
        [HttpGet("GetCategoryList")]
        public List<SelectListItem> GetCategoryList()
        {
            List<SelectListItem> categoryValues = (from x in _context.Categories.ToList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId.ToString()
                                                   }).ToList();
            return categoryValues;
        }
        //api/product/1
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _context.Products.Include(x => x.Category).Where(x => x.ProductId == id).FirstOrDefault();
        }
        //api/product/productModel
        [HttpPost("ProductAdd")]
        public Product ProductAdd([FromBody] Product product)
        {
            product.IsActive = true;
            _context.Products.AddAsync(product);
            _context.SaveChangesAsync();
            return product;
        }
        [HttpPut("UpdateProduct")]
        public Product UpdateProduct([FromBody] Product product)
        {
            _context.Products.Update(product);
            _context.SaveChangesAsync();
            return product;
        }
        [HttpDelete("{id}")]
        public void ProductDelete(int id)
        {
            var product = _context.Products.Find(id);
             _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}

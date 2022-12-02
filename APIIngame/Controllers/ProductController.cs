using APIIngame.Services;
using DataAccessLayer.Concrete;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIIngame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IUserService _userService;
        private Context _context;
        public ProductController(IUserService userService, Context context)
        {
            _context = context;
            _userService = userService;
        }

        //api/product
        [HttpGet]
        public List<Product> Get()
        {
            return _context.Products.ToList();
        }
        //api/product/1
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _context.Products.FirstOrDefault(x => x.ProductId == id);
        }
        //api/product/productModel
        [HttpPost]
        public Product Post([FromBody] Product product)
        {

            _context.Products.AddAsync(product);
            _context.SaveChangesAsync();
            return product;
        }
        [HttpPut]
        public Product Put([FromBody] Product product)
        {
            _context.Products.Update(product);
            _context.SaveChangesAsync();
            return product;
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
             _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}

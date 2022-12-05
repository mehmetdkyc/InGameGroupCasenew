using DataAccessLayer.Concrete;
using EntityLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIIngame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private Context _context;
        public CategoryController(Context context)
        {
            _context = context;
        }

        //api/product
        [HttpGet]
        public List<Category> Get()
        {
            //store procedure ile kategori ve alt kategoriyi joinledim.
            //var deneme= _context.Categories.FromSqlRaw("RecursiveSubCategory").ToList();
            return _context.Categories.ToList();
        }
        //api/product/1
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return _context.Categories.FirstOrDefault(x => x.CategoryId == id);
        }
        //api/product/productModel
        [HttpPost]
        public Category CategoryAdd([FromBody] Category category)
        {
            category.IsActive = true;
            _context.Categories.AddAsync(category);
            _context.SaveChangesAsync();
            return category;
        }
        [HttpPut("UpdateCategory")]
        public Category UpdateCategory([FromBody] Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChangesAsync();
            return category;
        }
        [HttpDelete("{id}")]
        public void CategoryDelete(int id)
        {
            var category = _context.Categories.Find(id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}

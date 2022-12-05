using APIIngame.Controllers;
using DataAccessLayer.Concrete;
using EntityLayer;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace xUnitForIngameGroup.Controller
{
    public  class ProductControllerTest
    {
        ProductController _productController;
        private Context _context;
        public ProductControllerTest()
        {
            _context = A.Fake<Context>();
            _productController =new ProductController(_context);

        }
        [Fact]
        public void ProductController_Index_ReturnSuccess()
        {
            //product index api test
            var products = A.Fake<List<Product>>();
            A.CallTo(() => _context.Products.ToList()).Returns(products);
            //Act
            var result = _productController.Get();
            //Assert
            result.Should().BeOfType<List<Product>>();
        }
    }
}

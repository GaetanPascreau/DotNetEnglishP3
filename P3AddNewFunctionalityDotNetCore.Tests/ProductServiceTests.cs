using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>
        [Fact]
        public void ExampleMethod()
        {
            // Arrange

            // Act


            // Assert
            Assert.Equal(1, 1);
        }

        // TODO write test methods to ensure a correct coverage of all possibilities

        [Fact]
        public void CheckFieldValidationWithMissingName()
        {
            // Arrange
            //we need to implement a new ProductService to be able to use the CheckProductModelErrors() method :
            //ProductService(ICart cart, IProductRepository productRepository, IOrderRepository orderRepository, IStringLocalizer stringLocalizer)
            // Mock all interfaces that are used in ProductService
            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IStringLocalizer<ProductService>> mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();
            //Mock<IProductService> mockProductService = new Mock<IProductService>();

            var errorName = new LocalizedString("MissingName", "Please enter a name");
            mockStringLocalizer.Setup(ml => ml["MissingName"]).Returns(errorName);
            ProductService productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockStringLocalizer.Object);
            ProductViewModel product = new ProductViewModel
            {
                Id = 1,
                Stock = "1",
                Price = "1",
                Name = null,
                Description = "x",
                Details = "y"
            };

            // Act
            var modelErrors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("Please enter a name", modelErrors);
        }

        [Fact]
        public void CheckFieldValidationWithMissingPrice()
        {
            // Arrange
            // Mock all interfaces that are used in ProductService
            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IStringLocalizer<ProductService>> mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();

            var errorName = new LocalizedString("MissingPrice", "Please enter a price");
            mockStringLocalizer.Setup(ml => ml["MissingPrice"]).Returns(errorName);
            ProductService productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockStringLocalizer.Object);
            ProductViewModel product = new ProductViewModel
            {
                Id = 1,
                Stock = "1",
                Price = null,
                Name = "z",
                Description = "x",
                Details = "y"
            };

            // Act
            var modelErrors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("Please enter a price", modelErrors);
        }

        [Fact]
        public void CheckFieldValidationWithPriceNotANumber()
        {
            // Arrange
            // Mock all interfaces that are used in ProductService
            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IStringLocalizer<ProductService>> mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();

            var errorName = new LocalizedString("PriceNotANumber", "The price must be a decimal value");
            mockStringLocalizer.Setup(ml => ml["PriceNotANumber"]).Returns(errorName);
            ProductService productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockStringLocalizer.Object);
            ProductViewModel product = new ProductViewModel
            {
                Id = 1,
                Stock = "1",
                Price = "a",
                Name = "z",
                Description = "x",
                Details = "y"
            };

            // Act
            var modelErrors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("The price must be a decimal value", modelErrors);
        }
    }
}
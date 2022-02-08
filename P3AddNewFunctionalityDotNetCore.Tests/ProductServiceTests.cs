using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
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

        // TODO write test methods to ensure a correct coverage of all possibilities

        [Fact]
        public void CheckFieldValidationWithMissingName()
        {
            // ARRANGE
            //we need to implement a new ProductService to be able to use the CheckProductModelErrors() method :
            //ProductService(ICart cart, IProductRepository productRepository, IOrderRepository orderRepository, IStringLocalizer stringLocalizer)
            // Mock all interfaces that are used in ProductService
            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IStringLocalizer<ProductService>> mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();

            //Associate an error message value ("Please enter a name") to the ["MissingName"] annotation
            var errorName = new LocalizedString("MissingName", "Please enter a name");
            mockStringLocalizer.Setup(ml => ml["MissingName"]).Returns(errorName);
            ProductService productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockStringLocalizer.Object);
            //create a new product with no name
            ProductViewModel product = new ProductViewModel
            {
                Id = 1,
                Stock = "1",
                Price = "10",
                Name = null,
                Description = "a product",
                Details = "details of the product"
            };

            // ACT
            //call the CheckProductModelErrors() method from ProductService to control fields validation and save the potential errors into the list
            var modelErrors = productService.CheckProductModelErrors(product);

            // ASSERT
            //control that the list of errors was updated with the right error message
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
                Stock = "12",
                Price = null,
                Name = "name of the product",
                Description = "another product",
                Details = "details from that product"
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
                Stock = "3",
                Price = "a",
                Name = "product name",
                Description = "this is a new product",
                Details = "the product's details"
            };

            // Act
            var modelErrors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("The price must be a decimal value", modelErrors);
        }

        [Fact]
        public void CheckFieldValidationWithPriceNotGreaterThanZero()
        {
            // Arrange
            // Mock all interfaces that are used in ProductService
            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IStringLocalizer<ProductService>> mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();

            var errorName = new LocalizedString("PriceNotGreaterThanZero", "The price must be greater than zero");
            mockStringLocalizer.Setup(ml => ml["PriceNotGreaterThanZero"]).Returns(errorName);
            ProductService productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockStringLocalizer.Object);
            ProductViewModel product = new ProductViewModel
            {
                Id = 1,
                Stock = "8",
                Price = "-1",
                Name = "product",
                Description = "a great product",
                Details = "a product with options"
            };

            // Act
            var modelErrors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("The price must be greater than zero", modelErrors);
        }

        [Fact]
        public void CheckFieldValidationWithMissingQuantity()
        {
            // Arrange
            // Mock all interfaces that are used in ProductService
            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IStringLocalizer<ProductService>> mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();

            var errorName = new LocalizedString("MissingQuantity", "Please enter a quantity");
            mockStringLocalizer.Setup(ml => ml["MissingQuantity"]).Returns(errorName);
            ProductService productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockStringLocalizer.Object);
            ProductViewModel product = new ProductViewModel
            {
                Id = 1,
                Stock = null,
                Price = "12",
                Name = "product2",
                Description = "description ot product2",
                Details = "details of product2"
            };

            // Act
            var modelErrors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("Please enter a quantity", modelErrors);
        }

        [Fact]
        public void CheckFieldValidationWithQuantityNotAnInteger()
        {
            // Arrange
            // Mock all interfaces that are used in ProductService
            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IStringLocalizer<ProductService>> mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();

            var errorName = new LocalizedString("StockNotAnInteger", "Quantity must be an integer");
            mockStringLocalizer.Setup(ml => ml["StockNotAnInteger"]).Returns(errorName);
            ProductService productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockStringLocalizer.Object);
            ProductViewModel product = new ProductViewModel
            {
                Id = 1,
                Stock = "1.2",
                Price = "5",
                Name = "product3",
                Description = "description of product3",
                Details = "lots of details from product3"
            };

            // Act
            var modelErrors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("Quantity must be an integer", modelErrors);
        }

        [Fact]
        public void CheckFieldValidationWithQuantityNotGreaterThanZero()
        {
            // Arrange
            // Mock all interfaces that are used in ProductService
            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IStringLocalizer<ProductService>> mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();

            var errorName = new LocalizedString("StockNotGreaterThanZero", "Quantity must be greater than zero");
            mockStringLocalizer.Setup(ml => ml["StockNotGreaterThanZero"]).Returns(errorName);
            ProductService productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockStringLocalizer.Object);
            ProductViewModel product = new ProductViewModel
            {
                Id = 1,
                Stock = "-1",
                Price = "8",
                Name = "the product name",
                Description = "a full description",
                Details = "more details on the product"
            };

            // Act
            var modelErrors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("Quantity must be greater than zero", modelErrors);
        }
    }
}
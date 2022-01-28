using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductIntegrationTests
    {
        [Fact]
        public void TestAllProductsInMemoryDatabase()
        {
            var connection = new ConnectionClass();
            using (var context = connection.CreateContextInMemory())
            {
                var product = new Product
                {
                    Price = 100.1,
                    Name = "new product",
                    Description = "x",
                    Details = "y",
                    Quantity = 10
                };
                var product2 = new Product
                {
                    Price = 87.4,
                    Name = "new product2",
                    Description = "t",
                    Details = "u",
                    Quantity = 120
                };

                var productService = new ProductRepository(context);
                productService.SaveProduct(product);
                productService.SaveProduct(product2);

                var results = productService.GetAllProducts();

                Assert.NotEmpty(results.ToList());
            }
        }

        [Fact]
        public void TestAllProductsInSQLSERVERDatabzse()
        {
            var connection = new ConnectionClass();
            using (var context = connection.CreateContextSQlDatabse())
            {
                var product = new Product
                {
                    Price = 100.1,
                    Name = "new product",
                    Description = "x",
                    Details = "y",
                    Quantity = 10
                };
                var product2 = new Product
                {
                    Price = 87.4,
                    Name = "new product2",
                    Description = "t",
                    Details = "u",
                    Quantity = 120
                };

                var productService = new ProductRepository(context);
                productService.SaveProduct(product);
                productService.SaveProduct(product2);

                var results = productService.GetAllProducts();

                //Assert.NotEmpty(results.ToList());
                Assert.NotNull(results.FirstOrDefault(p => p.Id == product.Id));
            }
        }

        [Fact]
        public void TestDeleteProductsInSQLSERVERDatabse()
        {
            var connection = new ConnectionClass();
            using (var context = connection.CreateContextSQlDatabse())
            {
                var product = new Product
                {
                    Price = 100.1,
                    Name = "new product",
                    Description = "x",
                    Details = "y",
                    Quantity = 10
                };
            
                var productService = new ProductRepository(context);
                productService.SaveProduct(product);
              

                var results = productService.GetAllProducts().ToList();
                foreach (var item in results)
                {
                    productService.DeleteProduct(item.Id);
                }

                var Data = productService.GetAllProducts().ToList();
                //Assert.NotEmpty(results.ToList());
                Assert.Empty(Data);
            }
        }

        [Fact]
        public void DataBackToPublicViews()
        {
            Mock<IProductService> mockProductServices = new Mock<IProductService>();
            Mock<ILanguageService> mocLanguage = new Mock<ILanguageService>();
            var productController = new ProductController(mockProductServices.Object, mocLanguage.Object);

            IActionResult ViewResult = productController.Index();

            Assert.IsType<ViewResult>(ViewResult);

        }

     

    }
    public class ConnectionClass : IDisposable
    {

        // create in memory connection context 
        public P3Referential CreateContextInMemory()
        {
            var option = new DbContextOptionsBuilder<P3Referential>().UseInMemoryDatabase(databaseName: "P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a").Options;
            var context = new P3Referential(option);

            // if context is not shown 
            if(context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;

        }

        // create helper connection using sql server to in memory database 
        public P3Referential CreateContextSQlDatabse()
        {
            var provider = new ServiceCollection().AddEntityFrameworkSqlServer().BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<P3Referential>();
            builder.UseSqlServer($"Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true").UseInternalServiceProvider(provider);

            var context = new P3Referential(builder.Options);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            return context;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposed)
        {
            if (disposed)
            {

            }
        }
    }
}

using System;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
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
        public void TestAllProductsInsideInMemoryDatabase()
        {
            //ARRANGE
            // create a new object from the ConnectionClass class added below and use its CreateContextInMemory() method to Create a connection to the in-memory database
            var connection = new ConnectionClass();
            using (var context = connection.CreateContextInMemory())
            {
                var product = new Product
                {
                    Price = 100.1,
                    Name = "new product",
                    Description = "product description",
                    Details = "product details",
                    Quantity = 10
                };
                var product2 = new Product
                {
                    Price = 87.4,
                    Name = "new product2",
                    Description = "product2 description",
                    Details = "product2 details",
                    Quantity = 120
                };

                //ACT
                //create the in-memory database and save the products inside of it
                var productService = new ProductRepository(context);
                productService.SaveProduct(product);
                productService.SaveProduct(product2);

                //recover all products from the in-memory database
                var results = productService.GetAllProducts();

                //ASSERT
                //Test that the products are really saved inside the in-memory database = the database is not empty
                Assert.NotEmpty(results.ToList());
            }
        }

        [Fact]
        public void TestAllProductsInsideSQLSERVERDatabase()
        {
            
            //ARRANGE
            // create a new object from the ConnectionClass class added below and use its CreateContextSQLSERVERDatabase() method to Create a connection to the SQL SERVER database
            var connection = new ConnectionClass();
            using (var context = connection.CreateContextSQlDatabase())
            {
                var product = new Product
                {
                    Price = 35.1,
                    Name = "new product",
                    Description = "new product description",
                    Details = "product details",
                    Quantity = 5
                };
                var product2 = new Product
                {
                    Price = 44.4,
                    Name = "new product2",
                    Description = "new product2 description",
                    Details = "product2 details",
                    Quantity = 24
                };

                //ACT
                //create the SQL SERVER database and save the products inside of it
                var productService = new ProductRepository(context);
                productService.SaveProduct(product);
                productService.SaveProduct(product2);

                //recover all products from the SQL server database
                var results = productService.GetAllProducts();

                //ASSERT
                //test that the products are persistently saved inside the SQL SERVER database, by making a new search inside of it
                Assert.NotNull(results.FirstOrDefault(p => p.Id == product.Id));
            }
        }

        [Fact]
        public void TestDeleteProductsInSQLSERVERDatabase()
        {
            //ARRANGE
            var connection = new ConnectionClass();
            using (var context = connection.CreateContextSQlDatabase())
            {
                var product = new Product
                {
                    Price = 75.5,
                    Name = "new product",
                    Description = "new product description",
                    Details = "new product details",
                    Quantity = 12
                };

                //ACT
                //create the SQL SERVER database and save the products inside of it
                var productService = new ProductRepository(context);
                productService.SaveProduct(product);
              
                //get all products from the database and delete them
                var results = productService.GetAllProducts().ToList();
                foreach (var item in results)
                {
                    productService.DeleteProduct(item.Id);
                }

                //get all products from the database again, inside a new list
                var Data = productService.GetAllProducts().ToList();

                //ASSERT
                //test that the product was saved inside the SQL SERVER database
                //Assert.NotNull(results.FirstOrDefault(p => p.Id == product.Id));
                //test that all products were deleted from the SQL SERVER database (= the database is empty) by checking the second list
                Assert.Empty(Data);
            }
        }

        [Fact]
        public void DataBackToPublicViews()
        {
            //ARRANGE
            //Mock all interfaces used in ProductController in order to use its Index() method, and create a new productController
            Mock<IProductService> mockProductServices = new Mock<IProductService>();
            Mock<ILanguageService> mocLanguage = new Mock<ILanguageService>();
            var productController = new ProductController(mockProductServices.Object, mocLanguage.Object);

            //ACT
            //use the Index() method from the ProductController class to return the product view
            IActionResult ViewResult = productController.Index();

            //ASSERT
            //control that the product view is of the right type
            Assert.IsType<ViewResult>(ViewResult);

        }
    }

    //Add a class containing the methods that create the connections
    //this class implement the IDisposable interface to free unmanaged resources (= close the connection) at the end of each test
    public class ConnectionClass : IDisposable
    {
        //indicate that the Dispose() method has already been run to prevent it from running while we create the connections
        private bool disposedValue;

        // create in memory connection context 
        public P3Referential CreateContextInMemory()
        {
            var options = new DbContextOptionsBuilder<P3Referential>().UseInMemoryDatabase(databaseName: "P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a").Options;
            var context = new P3Referential(options);

            // if context already exists, delete the database and recreate it 
            if(context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }


        // create sql server connection context 
        public P3Referential CreateContextSQlDatabase()
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

        //Add a Dispose() method to close the connection when the test is over and free resources
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ConnectionClass()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

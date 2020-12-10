using BankApp.Controllers;
using BankApp.Data;
using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BankApp.Tests.UnitTests
{
    public class AccountControllerTests
    {
        [Fact]
        public void Register_ReturnsAViewResult()
        {
            using (var context = GetApplicationContext())
            {
                // Arrange
                var controller = new AccountController(context);

                // Act
                var result = controller.Register();

                // Assert
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public void Login_ReturnsAViewResult()
        {
            using (var context = GetApplicationContext())
            {
                // Arrange
                var controller = new AccountController(context);

                // Act
                var result = controller.Login();

                // Assert
                Assert.IsType<ViewResult>(result);
            }
        }

        private ApplicationContext GetApplicationContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "UsersDatabase")
                .Options;

            var context = new ApplicationContext(options);
            DbInitializer.Initialize(context);
            return context;
        }
    }
}

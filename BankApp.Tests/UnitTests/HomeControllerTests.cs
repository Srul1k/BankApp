using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BankApp.Models;
using BankApp.Controllers;
using BankApp.Data;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Tests.UnitTests
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfUsers()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);

                var result = await controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<User>>(
                    viewResult.ViewData.Model);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public void Create_ReturnsAViewResult()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);

                var result = controller.Create();

                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task CreatePost_ReturnsARedirect_WhenModelStateIsValid()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);

                var result = await controller.Create(new User() { Name = "John" });

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Null(redirectToActionResult.ControllerName);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            }
        }

        [Fact]
        public async Task Details_ReturnsAViewResult_WithUserModel()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);

                var result = await controller.Details(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<User>(
                    viewResult.Model);
                Assert.Equal("chmel@mail.ru", model.Email);
            }
        }

        [Fact]
        public async Task Details_ReturnsNotFoundResult_WhenUserIsNotFound()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);

                var result = await controller.Details(-117013);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Edit_ReturnsAViewResult_WithUserModel()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);

                var result = await controller.Edit(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<User>(
                    viewResult.Model);
                Assert.Equal("chmel@mail.ru", model.Email);
            }
        }

        [Fact]
        public async Task Edit_ReturnsNotFoundResult_WhenUserIsNotFound()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);

                var result = await controller.Edit(-117013);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task EditPost_ReturnsARedirect_WhenUserWasEditedCorrect()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);
                var editedUser = await context.Users.FirstOrDefaultAsync(p => p.Id == 1);
                editedUser.Name = "Василий";

                var result = await controller.Edit(editedUser);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Null(redirectToActionResult.ControllerName);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.Equal("Василий", context.Users.Find(1).Name);
            }
        }

        [Fact]
        public async Task ConfirmDelete_ReturnsAViewResult_WithAUser()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);

                var result = await controller.ConfirmDelete(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<User>(
                    viewResult.Model);
                Assert.Equal("chmel@mail.ru", model.Email);
            }
        }

        [Fact]
        public async Task ConfirmDelete_ReturnsNotFoundResult_WhenUserIsNotFound()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);

                var result = await controller.ConfirmDelete(-117013);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeletePost_ReturnsARedirect_WhenUserStateIsDeleted()
        {
            using (var context = GetApplicationContext())
            {
                var controller = new HomeController(context);

                var result = await controller.Delete(1);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Null(redirectToActionResult.ControllerName);
                Assert.Equal("Index", redirectToActionResult.ActionName);
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BankApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BankApp.Controllers
{
    [Produces("application/json")]
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        /// <summary>
        /// Shows Index.cshtml page with all users
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        /// <summary>
        /// Get-method for creates User
        /// </summary>
        /// <returns>Redirects to the page Create.cshtml</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Adds User to database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Redirects to the page Index.cshtml</returns>
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Shows detailed User information.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirects to the page Details.cshtml</returns>
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                //User user = await db.Users.Include(a => a.UserAccounts).
                //    Include(b => b.UserRoles).FirstOrDefaultAsync(p => p.Id == id);
                User user = await db.Users.Include(a => a.UserAccounts)
           .Include(b => b.UserRoles)
           .ThenInclude(x => x.Role)
           .FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        /// <summary>
        /// Shows detailed User information for changes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirects to the page Edit.cshtml</returns>
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        /// <summary>
        /// Edits User in database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Redirects to the page Index.cshtml</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Shows short User description for deletes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirects to the page Delete.cshtml</returns>
        [Route("/Home/Delete/{id?}")]
        [HttpGet]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        /// <summary>
        /// Deletes User.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirects to the page Index.cshtml</returns>
        /// <response code="200">If the user is successfully deleted</response>
        /// <response code="404">If the user is not found</response>
        [Route("/Home/Delete/{id?}")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User user = new User { Id = id.Value };
                db.Entry(user).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}

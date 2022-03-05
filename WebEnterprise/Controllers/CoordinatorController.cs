using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Models.DTO;

namespace WebEnterprise.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly ApplicationDbContext context;

        public CoordinatorController(UserManager<CustomUser> _userManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            context = _context;
        }
        public IActionResult Index()
        {
            var coordinators = (from u in context.Users
                             join ur in context.UserRoles on u.Id equals ur.UserId
                             join r in context.Roles on ur.RoleId equals r.Id
                             where r.Name == "Coordinator"
                             select new UserDTO
                             {
                                 Id = u.Id,
                                 //FullName = u.FullName,
                                 UserName = u.UserName,
                                 Email = u.Email,
                             }).ToList();
            return View(coordinators);
        }

        public IActionResult AddCoordinator()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCoordinator(UserDTO user)
        {
            if (ModelState.IsValid)
            {
                var account = new CustomUser
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                };
                var result = await userManager.CreateAsync(account, "Abc@12345");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(account, "Coordinator");
                    TempData["message"] = $"Successfully Add new Coordinator {account.FullName}";
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        public IActionResult EditCoordinator(string id)
        {
            var coordinator = context.Users.FirstOrDefault(u => u.Id == id);
            if (coordinator == null)
            {
                return RedirectToAction("Index");
            }
            return View(coordinator);
        }

        [HttpPost]
        public IActionResult EditCoordinator(UserDTO coordinator)
        {
            if (ModelState.IsValid)
            {
                context.Entry(coordinator).State = EntityState.Modified;
                context.SaveChanges();
                TempData["message"] = $"Successfully Edit Coordinator {coordinator.FullName}";
                return RedirectToAction("Index");
            }
            return View(coordinator);
        }

        public IActionResult DeleteCoordinator(string id)
        {
            var coordinator = context.Users.FirstOrDefault(s => s.Id == id);
            if (coordinator == null)
            {
                return RedirectToAction("Index");
            }
            context.Remove(coordinator);
            context.SaveChanges();
            TempData["message"] = $"Successfully Delete Coordinator";
            return RedirectToAction("Index");
        }
    }
}

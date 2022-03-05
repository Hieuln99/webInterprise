using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Models.DTO;

namespace WebEnterprise.Controllers
{
    public class StaffController : Controller
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly ApplicationDbContext context;

        public StaffController(UserManager<CustomUser> _userManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            context = _context;
        }
        public IActionResult Index()
        {
            var staff = (from u in context.Users
                         join ur in context.UserRoles on u.Id equals ur.UserId
                         join r in context.Roles on ur.RoleId equals r.Id
                         where r.Name == "Staff"
                         select new UserDTO
                         {
                             Id = u.Id,
                             //FullName = u.FullName,
                             UserName = u.UserName,
                             Email = u.Email,
                         }).ToList();
            return View(staff);
        }

        public IActionResult AddStaff()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStaff(UserDTO user)
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
                    await userManager.AddToRoleAsync(account, "Staff");
                    TempData["message"] = $"Successfully Add new Staff {account.FullName}";
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        public IActionResult EditStaff(string id)
        {
            var staff = context.Users.FirstOrDefault(u => u.Id == id);
            if (staff == null)
            {
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        [HttpPost]
        public IActionResult EditStaff(UserDTO staff)
        {
            if (ModelState.IsValid)
            {
                context.Entry(staff).State = EntityState.Modified;
                context.SaveChanges();
                TempData["message"] = $"Successfully Edit Staff {staff.FullName}";
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        public IActionResult DeleteStaff(string id)
        {
            var staff = context.Users.FirstOrDefault(s => s.Id == id);
            if(staff == null)
            {
                return RedirectToAction("Index");
            }
            context.Remove(staff);
            context.SaveChanges();
            TempData["message"] = $"Successfully Delete Staff";
            return RedirectToAction("Index");
        }
    }
}

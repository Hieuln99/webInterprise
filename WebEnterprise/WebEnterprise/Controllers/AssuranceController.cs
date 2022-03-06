using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Models.DTO;

namespace WebEnterprise.Controllers
{
    public class AssuranceController : Controller
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly ApplicationDbContext context;

        public AssuranceController(UserManager<CustomUser> _userManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            context = _context;
        }
        public IActionResult Index()
        {
            var assurances = (from u in context.Users
                         join ur in context.UserRoles on u.Id equals ur.UserId
                         join r in context.Roles on ur.RoleId equals r.Id
                         where r.Name == "Assurance"
                         select new UserDTO
                         {
                             Id = u.Id,
                             FullName = u.FullName,
                             UserName = u.UserName,
                             Email = u.Email
                         }).ToList();
            return View(assurances);
        }


        [HttpPost]
        public async Task<IActionResult>AddAssurance(string userName, string fullName, string email)
        {
            if (ModelState.IsValid)
            {
                var account = new CustomUser
                {
                    UserName = userName,
                    FullName = fullName,
                    Email = email
                };
                var result = await userManager.CreateAsync(account, "Abc@12345");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(account, "Assurance");
                    TempData["message"] = $"Successfully Add new Assurance {account.FullName}";
                    return RedirectToAction("Index");
                }
            }
            return BadRequest();
        }

        public IActionResult EditAssurance(string id)
        {
            var assurance = context.Users.Where(u => u.Id == id).Select(u => new UserDTO
            {
                Email = u.Email,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                UserName = u.UserName
            }).FirstOrDefault();
            if (assurance == null)
            {
                return RedirectToAction("Index");
            }
            return View(assurance);
        }

        [HttpPost]
        public IActionResult EditAssurance(UserDTO res)
        {

            if (ModelState.IsValid)
            {
                var assurance = context.Users.Find(res.Id);
                if(assurance != null)
                {
                    assurance.Email = res.Email;
                    assurance.FullName = res.FullName;
                    assurance.UserName = res.UserName;
                    assurance.PhoneNumber = res.PhoneNumber;
                    context.SaveChanges();
                    TempData["message"] = $"Successfully Edit Assurance {assurance.FullName}";
                }
                return RedirectToAction("Index");
            }
            return View(res);
        }

        public IActionResult DeleteAssurance(string id)
        {
            var assurance = context.Users.FirstOrDefault(s => s.Id == id);
            if (assurance == null)
            {
                return RedirectToAction("Index");
            }
            context.Remove(assurance);
            context.SaveChanges();
            TempData["message"] = $"Successfully Delete Assurance";
            return RedirectToAction("Index");
        }
    }
}

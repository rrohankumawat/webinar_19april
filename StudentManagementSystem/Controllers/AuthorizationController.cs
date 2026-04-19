using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Dto;
using StudentManagementSystem.Persistence;

namespace StudentManagementSystem.Controllers
{
    public class AuthorizationController : Controller //MVCo,  Apicontroller 
    {
        private readonly AppDbContext _context;
        public AuthorizationController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult RegisterUser(UserDto dto)
        {

            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password) 
                || string.IsNullOrWhiteSpace(dto.CnfPassword))
            {
                return RedirectToAction("Registration");
            }

            if (dto.Password != dto.CnfPassword)
            {
                return RedirectToAction("Registration");
            }

            _context.Users.Add(new Models.User
            {
                Username = dto.Username,
                Password = dto.Password
            });

            _context.SaveChanges();
            return RedirectToAction("Login");
        }


        public IActionResult LoginUser(UserDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return RedirectToAction("Registration");
            }

            var user = _context.Users.FirstOrDefault(x => x.Username == dto.Username);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            if(user.Password != dto.Password)
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Index", "Dashboard");

        }

    }
}

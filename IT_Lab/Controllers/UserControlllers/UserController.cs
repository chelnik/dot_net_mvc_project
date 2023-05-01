using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using IT_Lab.Models;

namespace IT_Lab.Controllers.UserControlllers
{
    public class UserController : Controller
    {
        private static List<User> Users = new List<User>
        {
            new User { Login = "Alex2023@gmail.com", Password = "1234", 
                FullName = "Alex London", BirthDate = new DateTime(1999, 12, 12) },
            new User { Login = "Nikola2005@gmail.com", Password = "Nikola2025", 
                FullName = "Nikola Ivanov", BirthDate = new DateTime(2005, 1, 1) },
            new User { Login = "Michael1990@gmail.com", Password = "Mic$%2025", 
                FullName = "Michael Uzi", BirthDate = new DateTime(1990, 5, 15) }
        };

        [HttpPost]
        public IActionResult Login(User user)
        {
            var foundUser = Users.FirstOrDefault(u => u.Login == user.Login);

            if (foundUser == null)
            {
                ModelState.AddModelError("Login", "Incorrect login.");
                return View(user);
            }

            if (user.Password != foundUser.Password)
            {
                ModelState.AddModelError("Password", "Incorrect password.");
                return View(user);
            }

            Response.Cookies.Append("SignIn", foundUser.Login);

            return RedirectToAction("Account");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("SignIn"))
            {
                Response.Cookies.Delete("SignIn");
            }

            return RedirectToAction("Login");
        }

        public IActionResult Account()
        {
            var userLogin = Request.Cookies["SignIn"];

            if (userLogin == null)
            {
                return RedirectToAction("Login");
            }

            var user = Users.FirstOrDefault(u => u.Login == userLogin);
            return View(user);
        }
    }
}

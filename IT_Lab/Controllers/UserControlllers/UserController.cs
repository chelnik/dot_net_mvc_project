using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult Login(User user)
        {
            if (Request.Method == "GET" || user.Login == null || user.Password == null)
            {
                return View();
            }

            var found_user = Users.Find(u => u.Login == user.Login);

            if (found_user == null)
            {
                ModelState.AddModelError("Login", "Incorrect login.");
                return View(user);
            }

            if (user.Password != found_user.Password)
            {
                ModelState.AddModelError("Password", "Incorrect password.");
                return View(user);
            }

            Response.Cookies.Append("SignIn", found_user.Login);

            return RedirectToAction("Account");
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
            var user_login = Request.Cookies["SignIn"];

            if (user_login != null)
            {
                var user = Users.Find(u => u.Login == user_login);
                return View(user);
            }

            return RedirectToAction("Login");
        }
    }
}

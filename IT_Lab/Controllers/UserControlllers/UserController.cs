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
        // создаем список пользователей
        private static List<User> Users = new List<User>
        {
            new User { Login = "Alex2023@gmail.com", Password = "1234", 
                FullName = "Alex London", BirthDate = new DateTime(1999, 12, 12) },
            new User { Login = "Nikola2005@gmail.com", Password = "Nikola2025", 
                FullName = "Nikola Ivanov", BirthDate = new DateTime(2005, 1, 1) },
            new User { Login = "Michael1990@gmail.com", Password = "Mic$%2025", 
                FullName = "Michael Uzi", BirthDate = new DateTime(1990, 5, 15) }
        };

        // метод для входа пользователя
        public IActionResult Login(User user)
        {
            // если метод GET или пользователь не ввел логин и пароль
            if (Request.Method == "GET" || user.Login == null || user.Password == null)
            {
                return View(); // отображаем форму для ввода логина и пароля
            }

            // ищем пользователя с таким логином
            var found_user = Users.Find(u => u.Login == user.Login);

            // если такой пользователь не найден
            if (found_user == null)
            {
                ModelState.AddModelError("Login", "Incorrect login."); // добавляем ошибку модели
                return View(user); // отображаем форму с ошибкой
            }

            // если пароль не совпадает с паролем пользователя
            if (user.Password != found_user.Password)
            {
                ModelState.AddModelError("Password", "Incorrect password."); // добавляем ошибку модели
                return View(user); // отображаем форму с ошибкой
            }

            // добавляем куки с логином пользователя
            Response.Cookies.Append("SignIn", found_user.Login);

            return RedirectToAction("Account"); // перенаправляем на страницу аккаунта
        }

        // метод для выхода пользователя
        public IActionResult Logout()
        {
            // если куки содержит логин пользователя
            if (Request.Cookies.ContainsKey("SignIn"))
            {
                Response.Cookies.Delete("SignIn"); // удаляем куки
            }

            return RedirectToAction("Login"); // перенаправляем на страницу входа
        }

        // метод для отображения аккаунта пользователя
        public IActionResult Account()
        {
            var user_login = Request.Cookies["SignIn"]; // получаем логин из куков

            // если логин есть
            if (user_login != null)
            {
                var user = Users.Find(u => u.Login == user_login); // ищем пользователя в списке
                return View(user); // отображаем страницу аккаунта с данными пользователя
            }

            return RedirectToAction("Login"); // иначе перенаправляем на страницу входа
        }
    }
}

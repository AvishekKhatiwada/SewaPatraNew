using Microsoft.AspNetCore.Mvc;
using SewaPatra.BusinessLayer;
using SewaPatra.Models;

namespace SewaPatra.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRegisterService _userRegisterService;
        public AccountController(UserRegisterService userRegisterService)
        {
            _userRegisterService = userRegisterService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegister userRegister)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_userRegisterService.InsertUser(userRegister))
                    {
                        TempData["Message"] = "User Registered Successfully!";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username or Email already exists.");
                        TempData["Message"] = "Username or Email already exists!";
                    }
                }
                return RedirectToAction("Register");
            }
            catch
            {
                TempData["Message"] = "User Registration Failed!";
                return RedirectToAction("Register");
            }
        }
        public ActionResult Login()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model)
        {
            ViewBag.Message = TempData["Message"];
            if (ModelState.IsValid)
            {
                UserRegister user = _userRegisterService.AuthenticateUser(model.Number, model.Password);

                if (user != null)
                {
                    //FormsAuthentication.SetAuthCookie(model.Username, false);
                    // Successful login
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.FullName);
                    HttpContext.Session.SetString("userRole", user.Role.ToString());
                    HttpContext.Session.SetString("Number", user.Number.ToString());
                    //string id = HttpContext.Session.GetString("UserId");
                    //string name = HttpContext.Session.GetString("Username");
                    //string role = HttpContext.Session.GetString("UserRole");
                    //string number = HttpContext.Session.GetString("Number");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    model.Number = ""; // or model.Number = "";
                    model.Password = "";
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            // Clear session variables
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("userRole");

            // Redirect to the login page or home page
            return RedirectToAction("Login", "Account"); // Assuming your login action is in AccountController
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(Login model)
        {
            if (ModelState.IsValid)
            {
                //Get the user from the database
                //UserRegister user = _userRegisterService.GetUserByNumber(model.Number);
                UserRegister user = _userRegisterService.AuthenticateUser(model.Number, model.Password);
                if (user != null)
                {
                    if (model.NewPassword == model.ConfirmPassword)
                    {
                        if (model.Password != model.NewPassword)
                        {
                            // Change the password
                            user.Password = UserRegister.HashPassword(model.NewPassword);
                            if (_userRegisterService.UpdateUserPassword(user))
                            {
                                TempData["Message"] = "Password changed successfully!";
                                return View(model);
                            }
                            else
                            {
                                TempData["Message"] = "Something went wrong. Please try again later!!";
                                return View(model);
                            }
                        }
                        else
                        {
                            TempData["Message"] = "New password must be different from the old password!!";
                        }
                    }
                    else
                    {
                        TempData["Message"] = "New password and confirm password do not match!!";
                    }
                }
                else
                {
                    TempData["Message"] = "The current password you entered is incorrect!!";
                }
            }
            return View(model);
        }
    }
}

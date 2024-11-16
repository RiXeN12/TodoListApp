using Microsoft.AspNetCore.Mvc;

using TodoListApp.Models;
using Microsoft.AspNetCore.Identity;
using TodoListApp.Repositories.AccountRepository;
using Microsoft.AspNetCore.Authorization;
namespace TodoListApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(IAccountRepository accountRepository, IPasswordHasher<User> passwordHasher)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user, string password)
        {
            if (!ModelState.IsValid)
                return View(user);

            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            await _accountRepository.AddAsync(user);
            HttpContext.Session.SetInt32("UserId", user.Id);
            TempData["Toast"] = "Account created successfully!";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _accountRepository.GetUserByEmailAsync(email);
            if (user == null ||
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Success)
            {

                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }
            else
            {
                TempData["Toast"] = "Welcome back!";
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var user = await _accountRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return NotFound();

            var model = new EditProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var user = await _accountRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return NotFound();

            user.UserName = model.UserName;
            user.Email = model.Email;

            await _accountRepository.UpdateAsync(user);
            TempData["Toast"] = "Profile updated successfully!";
            return RedirectToAction("EditProfile");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var user = await _accountRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return NotFound();

            var verificationResult = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, model.CurrentPassword);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Current password is incorrect");
                return View(model);
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
            await _accountRepository.UpdateAsync(user);
            TempData["Toast"] = "Password changed successfully!";
            return RedirectToAction("EditProfile");
        }
    }
}


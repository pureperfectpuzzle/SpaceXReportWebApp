using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdentityController(UserManager<IdentityUser> userManager, 
			RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
			this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public IActionResult DisplayLoginView(string returnUrl = "")
        {
			return View("Login", new LoginViewModel { ReturnUrl = returnUrl });
        }

		[HttpPost]
		public async Task<IActionResult> Login(string userName, string password, string returnUrl = "")
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await this._signInManager.PasswordSignInAsync(userName, password, false, false);
				if (result.Succeeded)
				{
					return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
				}

				ModelState.AddModelError("", "Invalid username or password");
			}

			return View("Login", new LoginViewModel { UserName = userName, ReturnUrl = returnUrl });
		}

		public async Task<IActionResult> Logout()
		{
			await this._signInManager.SignOutAsync();
			return Redirect("/");
		}

		public IActionResult AccessDenied()
		{
			return View();
		}

		public IActionResult DisplaySignUpView()
		{
			return View("SignUpView", new UserAccountSignUpModel());
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(UserAccountSignUpModel model)
		{
			if (ModelState.IsValid)
			{
				IdentityUser user = new IdentityUser { UserName = model.UserName, Email = model.Email };
				IdentityResult result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					result = await _userManager.AddToRoleAsync(user, "Users");
					if (result.Succeeded)
					{
						return RedirectToAction(nameof(SignUpResult));
					}
				}

				foreach (IdentityError err in result.Errors)
				{
					ModelState.AddModelError("", err.Description);
				}
			}
			return View("SignUpView", model);
		}

		public IActionResult SignUpResult()
		{
			return View();
		}
	}
}

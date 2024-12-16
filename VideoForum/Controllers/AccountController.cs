using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VideoForum.Core.Entities;
using VideoForum.ViewModels.Account;

namespace VideoForum.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> userManager;
    private readonly SignInManager<AppUser> signInManager;

    public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        Login_vm loginVM = new()
        {
            ReturnUrl = returnUrl,
        };
        return View(loginVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Login_vm model)
    {
        if (ModelState.IsValid is false)
        {
            return View(model);
        }
        else
        {
            //if there is no return url, set its value as the root application url
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");

            AppUser? user;

            if (await userManager.FindByNameAsync(model.UserName) is null)
            {
                user = await userManager.FindByEmailAsync(model.UserName);
            }
            else if (await userManager.FindByNameAsync(model.UserName) is not null)
            {
                user = await userManager.FindByNameAsync(model.UserName);
            }
            else
            {
                user = null;
            }

            //If username and email cannot be found
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password, try again please");
                return View(model);
            }

            //check the password
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);


            if (result.Succeeded)
            {
                await HandleSignInUserAsync(user);
                return LocalRedirect(model.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password, try again please");
                return View(model);
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");

    }



    #region Private Methods
    /// <summary>
    /// Assign some claims to the user and sign the user in using cookie authentication
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task HandleSignInUserAsync(AppUser user)
    {
        //Create a claims identity instance
        ClaimsIdentity claimsIdentity = new (CookieAuthenticationDefaults.AuthenticationScheme);

        //Add several claims to the claims identity
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

        //Get all the roles a user is in
        var roles = await userManager.GetRolesAsync(user);

        //Create and add a claim for each role to the claims identity
        claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var principal = new ClaimsPrincipal(claimsIdentity);

        //Use this method to assign identity claims into User.Identity and sign the user in using the built-in .NET identity
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
    #endregion

}

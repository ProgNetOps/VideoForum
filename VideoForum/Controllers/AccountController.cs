using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using VideoForum.Core.Entities;
using VideoForum.Utility;
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
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register_vm model)
    {
        if (ModelState.IsValid)
        {

            //check if email already exists
            if (await CheckEmailExistsAsync(model.Email) is true)
            {
                ModelState.AddModelError("Email", $"Email address '{model.Email}' is taken, please try with a different email");

                return View(model);
            }

            //check if the username already exists
            if (await CheckNameExistsAsync(model.Name) is true)
            {
                ModelState.AddModelError("Name", $"Username '{model.Name}' is taken, please try with a different one");

                return View(model);
            }

            AppUser userToAdd = new()
            {
                Name = model.Name,
                UserName = model.Name?.ToLower(),
                Email = model.Email?.ToLower()
            };

            var result = await userManager.CreateAsync(userToAdd, model.Password);

            //Add a new user to User Role (Not Admin or Moderator)
            await userManager.AddToRoleAsync(userToAdd, SD.UserRole);

            if (result.Succeeded is false)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError( string.Empty, error.Description);
                }
                return View(model);
            }
            else
            {
                await HandleSignInUserAsync(userToAdd);
                return RedirectToAction("Index", "Home");
            }
        }
        else
        {
            return View(model);
        }
    }


    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(); 
        return RedirectToAction("Index", "Home");

    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }


    #region Private Methods

    private async Task<bool> CheckEmailExistsAsync(string email)
    {
        return await userManager.Users.AnyAsync(x => x.Email == email.ToLower());
    }

    private async Task<bool> CheckNameExistsAsync(string name)
    {
        return await userManager.Users.AnyAsync(x => x.Name.ToLower() == name.ToLower());
    }
        /// <summary>
    /// Assign some claims to the user and sign the user in using cookie authentication
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task HandleSignInUserAsync(AppUser user)
    {
        //-1- Create a claims identity instance
        ClaimsIdentity claimsIdentity = new (CookieAuthenticationDefaults.AuthenticationScheme);

        //-2- Add several claims to the claims identity
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

        //-3- Get all the roles a user is in
        var roles = await userManager.GetRolesAsync(user);

        //-4- Create and add a claim for each role to the claims identity
        claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        //-5- Create a claims principal instance and pass it the claims identity instance that already has several claims
        ClaimsPrincipal principal = new (claimsIdentity);

        //-6- Use this method to assign identity claims into User.Identity and sign the user in using the built-in .NET identity
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
    #endregion

}

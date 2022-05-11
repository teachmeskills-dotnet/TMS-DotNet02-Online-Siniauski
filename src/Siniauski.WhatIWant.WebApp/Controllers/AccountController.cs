using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Siniauski.WhatIWant.WebApp.Attributes;
using Siniauski.WhatIWant.WebApp.Interfaces;
using Siniauski.WhatIWant.WebApp.ViewModels;
using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;
using Siniauski.WhatIWant.WebData.Enums;
using Siniauski.WhatIWant.WebData.Models;
using System.Security.Claims;

namespace Siniauski.WhatIWant.WebApp.Controllers
{
    [NoDirectAccess]
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IWebHostEnvironment _appEnvironment;

        public AccountController(IIdentityService identityService, IWebHostEnvironment appEnvironment)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            LoginViewModel loginViewModel = new();
            return View(loginViewModel);
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(UserLoginRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserAuthModel userAuthModel = (UserAuthModel)await _identityService.LoginAsync(request);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userAuthModel.Id),
                        new Claim(ClaimTypes.Name, userAuthModel.UserName),
                        new Claim(ClaimTypes.Sid, userAuthModel.Token),
                        new Claim(ClaimTypes.IsPersistent, "false"),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string message = "";
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            message += error.ErrorMessage + "\n";
                        }
                    }
                    message = message.TrimEnd('\n');
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Messages = ex.Message.Split('\n');
            }
            return View(new LoginViewModel() { Login = request.Login, Password = request.Password });
        }

        [HttpPost]
        [Route("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                ViewBag.Messages = ex.Message.Split('\n');
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new();
            return View(registerViewModel);
        }

        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(UserRegisterRequest request)
        {
            string? avatarTemp = request.Avatar;
            try
            {
                if (ModelState.IsValid)
                {
                    request.Avatar = request.AvatarServerName;
                    UserAuthModel userAuthModel = (UserAuthModel)await _identityService.RegisterAsync(request);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userAuthModel.Id),
                        new Claim(ClaimTypes.Name, userAuthModel.UserName),
                        new Claim(ClaimTypes.Sid, userAuthModel.Token)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string message = "";
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            message += error.ErrorMessage + "\n";
                        }
                    }
                    message = message.TrimEnd('\n');
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Messages = ex.Message.Split('\n');
            }
            return View(new RegisterViewModel()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Login = request.Login,
                Email = request.Email,
                Avatar = avatarTemp,
                AvatarServerName = request.AvatarServerName,
                BirthDate = request.BirthDate,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber
            });
        }

        [HttpGet]
        [Route("Edit")]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditAsync()
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost("Account/UploadAvatar")]
        public object UploadAvatar()
        {
            try
            {
                foreach (var formFile in Request.Form.Files)
                {
                    var fullPath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot\\img\\Avatars", formFile.Name);
                    using FileStream fs = System.IO.File.Create(fullPath);
                    formFile.CopyTo(fs);
                    fs.Flush();
                }
                return new Response() { Status = ResponseStatus.Success, Message = "Файл успешно загружен!" };
            }
            catch
            {
                return new Response() { Status = ResponseStatus.Failure, Message = "Ошибка при загрузке файла!" };
            }
        }
    }
}
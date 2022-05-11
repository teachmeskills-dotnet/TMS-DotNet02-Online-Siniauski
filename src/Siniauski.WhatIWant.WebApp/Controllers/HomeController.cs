using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Siniauski.WhatIWant.WebApp.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Siniauski.WhatIWant.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && userName != null)
            {
                ViewBag.UserName = userName;
                return View();                
            }
            else
            {
                return RedirectToAction("Start");
            }            
        }

        [Route("Home")]
        public IActionResult Start()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartCMC.STS.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace ShoppingCartCMC.STS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        //PW: dependency injection - parameter interaction passed
        public HomeController(
            IIdentityServerInteractionService interaction,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _interaction = interaction;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error(string errorId)
        {
            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                _logger.LogInformation("IdentityServer error, Request IP: {ip}", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
            }

            return View();
        }
    }
}

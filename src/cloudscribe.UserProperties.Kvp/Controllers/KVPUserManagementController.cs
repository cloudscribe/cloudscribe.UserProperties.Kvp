using cloudscribe.Common.Gdpr;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Controllers.Mvc;
using cloudscribe.UserProperties.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.UserProperties.Kvp.Controllers
{
    public class KVPUserManagementController : Controller
    {
        protected readonly SiteUserManager<SiteUser> UserManager;
        protected readonly IUserPropertyService UserPropertyService;
        protected readonly ILogger<ManageController> Log;

        public KVPUserManagementController(
                        SiteUserManager<SiteUser> userManager,
                        IUserPropertyService      userPropertyService,
                        ILogger<ManageController> logger
                        )
        {
            UserManager         = userManager;
            UserPropertyService = userPropertyService;
            Log                 = logger;
        }

        [Authorize]
        [HttpGet]
        public virtual IActionResult PersonalData()
        {
            string userId = User.GetUserId();
            return View("PersonalDataWithKVP", userId);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DownloadPersonalDataWithKVP()
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            Log.LogInformation("User with ID '{UserId}' asked for their personal data.", UserManager.GetUserId(User));

            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(SiteUser).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataMarkerAttribute)));

            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await UserManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            personalData.Add($"Authenticator Key", await UserManager.GetAuthenticatorKeyAsync(user));

            var locations = await UserManager.GetUserLocations(user.SiteId, user.Id, 1, 100);
            int i = 1;
            foreach (var location in locations.Data)
            {
                personalData.Add($"IpAddress {i}", location.IpAddress);
                i += 1;
            }

            // add in the custom KVP
            foreach (var item in await UserPropertyService.FetchByUser(user.SiteId.ToString(), user.Id.ToString()))
            {
                if(!personalData.ContainsKey(item.Key))
                    personalData.Add(item.Key, item.Value);
            }

            var fileName = Request.Host.Host.Replace(":", "") + "-PersonalData.json";

            Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
            return new FileContentResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(personalData)), "text/json");
        }
    }
}

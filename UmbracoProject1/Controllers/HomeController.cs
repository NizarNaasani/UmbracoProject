using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoProject1.Models;

namespace UmbracoProject1.Controllers
{
    public class HomeController : SurfaceController
    {
        public HomeController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }


        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //instantiate a new MimeMessage
                    var message = new MimeMessage();

                    message.From.Add(new MailboxAddress(model.Name, "umbraco.test1@gmail.com"));
                    //Setting the From e-mail address
                    message.To.Add(new MailboxAddress("Mr Smith", model.Email));
                    //E-mail subject 
                    message.Subject = model.Subject;
                    //E-mail message body

                    message.Body = new TextPart(TextFormat.Html)
                    {
                        Text = model.Message
                    };

                    //Configure the e-mail
                    using (var emailClient = new SmtpClient())
                    {
                        await emailClient.ConnectAsync("pop.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                        await emailClient.AuthenticateAsync("umbraco.test1@gmail.com", "test123nizar1996");
                        await emailClient.SendAsync(message);
                        await emailClient.DisconnectAsync(true);
                    }
                    TempData["ContactSuccess"] = true;
                    return CurrentUmbracoPage();
                }
                catch(Exception ex)
                {
                    return RedirectToCurrentUmbracoPage();
                }
            }
            return CurrentUmbracoPage();
        }

    }
}

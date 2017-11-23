﻿using Abp.AspNetCore.Mvc.Authorization;
using Satrabel.OpenApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Satrabel.Starter.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ClientAppController : StarterControllerBase
    {
        public ActionResult Run(string id)
        {
            return View("Run", id);
        }
    }


    /// <summary>
    /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
    /// Don't use this code in production !!!
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>

    //public async Task<ActionResult> TestNotification(string message = "")
    //{
    //    if (message.IsNullOrEmpty())
    //    {
    //        message = "This is a test notification, created at " + Clock.Now;
    //    }

    //    var defaultTenantAdmin = new UserIdentifier(1, 2);
    //    var hostAdmin = new UserIdentifier(null, 1);

    //    await _notificationPublisher.PublishAsync(
    //            "App.SimpleMessage",
    //            new MessageNotificationData(message),
    //            severity: NotificationSeverity.Info,
    //            userIds: new[] { defaultTenantAdmin, hostAdmin }
    //         );

    //    return Content("Sent notification: " + message);
    //}
}
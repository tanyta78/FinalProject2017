using AuctionHub.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionHub.Web.Controllers
{
    public class BaseController : Controller
    {
        protected void ShowNotification(NotificationType type, string message)
        {
            this.TempData[Constants.NotificationMessageKey] = message;
            this.TempData[Constants.NotificationTypeKey] = type.ToString();
        }
    }
}

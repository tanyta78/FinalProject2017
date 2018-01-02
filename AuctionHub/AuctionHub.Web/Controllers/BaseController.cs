namespace AuctionHub.Web.Controllers
{
    using Common;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        protected void ShowNotification(NotificationType type, string message)
        {
            this.TempData[Constants.NotificationMessageKey] = message;
            this.TempData[Constants.NotificationTypeKey] = type.ToString();
        }
    }
}

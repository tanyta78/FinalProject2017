namespace AuctionHub.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System.Threading.Tasks;

    public class CommentController : BaseController
    {
        private readonly ICommentService comments;
        private readonly UserManager<User> userManager;

        public CommentController(
            ICommentService comments,
            UserManager<User> userManager)
        {
            this.comments = comments;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(int id, string comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = this.userManager.GetUserId(User);

            await this.comments.AddAsync(comment, userId, id);

            return RedirectToAction(nameof(AuctionController.Details), "Auction", new { id });
        }
    }
}

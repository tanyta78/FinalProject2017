namespace AuctionHub.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System;
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
        [HttpGet]
        public async Task<IActionResult> Add(int id, string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                return BadRequest("Comment cannot be empty");
            }

            var userId = this.userManager.GetUserId(User);
            var publishDate = DateTime.UtcNow;
            await this.comments.AddAsync(comment, userId, id, publishDate);

            return Ok(publishDate.ToShortDateString());

            //return RedirectToAction(nameof(AuctionController.Details), "Auction", new { id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.comments.DeleteAsync(id);

            return RedirectToAction(nameof(AuctionController.Details), "Auction", new { id });
        }
    }
}

namespace AuctionHub.Web.Controllers
{
    using AuctionHub.Web.Models.CommentViewModels;
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
            int newCommentId = await this.comments.AddAsync(comment, userId, id, publishDate);

            return Ok(new { publishDate = publishDate.ToShortDateString(), newCommentId });

            //return RedirectToAction(nameof(AuctionController.Details), "Auction", new { id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            Comment target = this.comments.GetById(id);

            if (target.AuthorId != userManager.GetUserId(User))
            {
                return Forbid("You do not have permission to delete this comment");
            }

            await this.comments.DeleteAsync(id);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(CommentEditViewModel model)
        {
            Comment target = this.comments.GetById(model.Id);

            if (target.AuthorId != userManager.GetUserId(User))
            {
                return Forbid("You do not have permission to edit this comment");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid post data");
            }

            this.comments.Edit(model.Id, model.NewContent);

            return Ok();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionHub.Web.Models.CommentViewModels
{
    public class CommentEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(2048)]
        public string NewContent { get; set; }
    }
}

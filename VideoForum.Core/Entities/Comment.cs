using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoForum.Core.Entities
{
    public class Comment
    {

        //PK (AppUserId, VideoId)
        //FK = AppUserId and VideoId

        public int AppUserId { get; set; }
        public int VideoId { get; set; }
        [Required]
        public string? Content { get; set; }
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        //Navigations
        public AppUser? AppUser { get; set; }
        public Video? Video { get; set; }
    }
}

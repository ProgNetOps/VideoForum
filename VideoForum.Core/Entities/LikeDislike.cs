using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoForum.Core.Entities
{
    public class LikeDislike
    {
        //PK (AppUserId, VideoId)
        //FK = AppUserId and VideoId

        public int AppUserId { get; set; }
        public int VideoId { get; set; }
        public bool Liked { get; set; }

        //Navigations
        public AppUser? AppUser { get; set; }
        public Video? Video { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoForum.Core.Entities
{
    public class Channel:BaseEntity
    {
        [Required]
        public string? Name { get; set; }
        public string? About { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int AppUserId { get; set; }

        //Navigations
        public AppUser? AppUser { get; set; }
        public ICollection<Video>? Videos { get; set; }
        public ICollection<Subscribe>? Subscribers { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoForum.Core.Entities
{
    public class Video: BaseEntity
    {
        [Required]
        public string? ThumbnailUrl { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? ContentType { get; set; }
        [Required]
        public byte[] Contents { get; set; }
        public int CategoryId { get; set; }
        public int ChannelId { get; set; }

        //Navigations
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        [ForeignKey(nameof(ChannelId))]
        public Channel? Channel { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<LikeDislike>? LikeDislikes { get; set; }
    }
}

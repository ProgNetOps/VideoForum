using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoForum.Core.Entities
{
    public class Category:BaseEntity
    {
        [Required]
        public string? Name { get; set; }
        public ICollection<Video>? Videos { get; set; }
    }
}

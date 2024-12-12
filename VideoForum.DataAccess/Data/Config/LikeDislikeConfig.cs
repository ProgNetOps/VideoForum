using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoForum.Core.Entities;

namespace VideoForum.DataAccess.Data.Config;

public class LikeDislikeConfig : IEntityTypeConfiguration<LikeDislike>
{
    public void Configure(EntityTypeBuilder<LikeDislike> builder)
    {
        //Defining the primary key which is a combination of AppUserId and VideoId

        //PRIMARY KEY
        builder.HasKey(x => new
        {
            x.AppUserId,
            x.VideoId
        });

        //FOREIGN KEY
        //Defining foreign keys
        builder.HasOne(a => a.AppUser)
            .WithMany(c => c.LikeDislikes)
            .HasForeignKey(c => c.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        //FOREIGN KEY
        builder.HasOne(a => a.Video)
            .WithMany(c => c.LikeDislikes)
            .HasForeignKey(c => c.VideoId)
            .OnDelete(DeleteBehavior.Restrict);

        //NOTE: We cannot have more than one cascade delete in SQL Server; an exception will be thrown, so we use restrict for the other ones. However, multiple delete cascade behaviour is allowed in some other databases eg PostGresql
    }
}

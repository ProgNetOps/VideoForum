using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VideoForum.Core.Entities;
using VideoForum.DataAccess.Data.Config;

namespace VideoForum.DataAccess.Data;

public class AppDbContext:IdentityDbContext<AppUser,AppRole,int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

    public DbSet<Category> Category => Set<Category>();
    public DbSet<Channel> Channel => Set<Channel>();
    public DbSet<Video> Video => Set<Video>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Shorter way of applying manual configuration
        // $ builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())

        //Longer way of applying manual configuration
        builder.ApplyConfiguration(new CommentConfig());
        builder.ApplyConfiguration(new SubscribeConfig());
        builder.ApplyConfiguration(new LikeDislikeConfig());
    }
}

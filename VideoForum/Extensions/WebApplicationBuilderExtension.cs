using Microsoft.EntityFrameworkCore;
using VideoForum.DataAccess.Data;

namespace VideoForum.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        return builder;
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoForum.DataAccess.Data;

//This class is responsible for updating the database; migrations, seeding the database etc
public static class ContextInitializer
{
    public static void Initialize(AppDbContext appDbContext)
    {
        if (appDbContext.Database.GetPendingMigrations().Count() > 0)
        {
            appDbContext.Database.Migrate();
        }
    }
}

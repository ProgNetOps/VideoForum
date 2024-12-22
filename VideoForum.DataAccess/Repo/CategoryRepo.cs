using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoForum.Core.Entities;
using VideoForum.Core.IRepo;
using VideoForum.DataAccess.Data;

namespace VideoForum.DataAccess.Repo;

public class CategoryRepo : BaseRepo<Category>, ICategoryRepo
{
    public CategoryRepo(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}

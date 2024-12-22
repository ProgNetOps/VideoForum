using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoForum.Core.Entities;
using VideoForum.Core.IRepo;
using VideoForum.DataAccess.Data;

namespace VideoForum.DataAccess.Repo
{
    public class ChannelRepo:BaseRepo<Channel>, IChannelRepo
    {
        public ChannelRepo(AppDbContext appDbContext):base(appDbContext)
        {          
        }
    }
}

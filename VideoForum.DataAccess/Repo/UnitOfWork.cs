using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoForum.Core.IRepo;
using VideoForum.DataAccess.Data;

namespace VideoForum.DataAccess.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public IChannelRepo ChannelRepo => new ChannelRepo(appDbContext);
        public ICategoryRepo CategoryRepo => new CategoryRepo(appDbContext);
        public IVideoRepo VideoRepo => new VideoRepo(appDbContext);

        public async Task<bool> CompleteAsync()
        {
            bool result = false;

            if(appDbContext.ChangeTracker.HasChanges() is true)
            {
                result = await appDbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        public void Dispose()
        {
            appDbContext.Dispose();
        }
    }
}

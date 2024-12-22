using Microsoft.AspNetCore.Mvc;
using VideoForum.Core.IRepo;

namespace VideoForum.Controllers
{
    public class CoreController : Controller
    {
        private IUnitOfWork? _unitOfWork;
        protected IUnitOfWork? UnitOfWork => _unitOfWork ??= HttpContext.RequestServices.GetService<IUnitOfWork>();

    }
}

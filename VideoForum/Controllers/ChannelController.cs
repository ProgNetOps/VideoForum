using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoForum.Utility;

namespace VideoForum.Controllers;

[Authorize(Roles =$"{SD.UserRole}")]
public class ChannelController : CoreController
{
    public IActionResult Index()
    {
        return View();
    }
}

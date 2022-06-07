namespace EM.Web.Areas.Dashboard.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static EM.Common.RoleConstants;

    [Area("Admin")]
    [Authorize(Roles = Admin)]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }
    }
}

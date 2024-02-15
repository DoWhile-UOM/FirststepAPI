using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

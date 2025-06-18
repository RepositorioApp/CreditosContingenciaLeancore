using Microsoft.AspNetCore.Mvc;

namespace CreditosContingencia.Controllers
{
    public class CreditoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

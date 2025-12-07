using Microsoft.AspNetCore.Mvc;

namespace X.Swashbuckle.Demo.Controllers;

public class HomeController : Controller
{
    public ActionResult Index()
    {
        return Redirect("~/swagger");
    }
}

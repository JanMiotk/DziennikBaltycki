using Microsoft.AspNetCore.Mvc;

namespace IntegrationApi.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult ErrorsService(int statusCode)
        {
            switch(statusCode)
            {
                case 404:
                    return NotFound("Nie znaleziono strony");
            }
            return View();
        }
    }
}
using FA.JustBlog.Core.Infrastructures;
using FA.JustBlog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace FA.JustBlog.Web.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var listPost = unitOfWork.PostRepository.GetAll(includeProperties:"Category");
            return View(listPost);
        }

        public IActionResult About()
        {
           
            return View();
        }
        public IActionResult Contact()
        {

            return View();
        }
        public ActionResult AboutCard()
        {
            return PartialView("_PartialAboutCard");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}
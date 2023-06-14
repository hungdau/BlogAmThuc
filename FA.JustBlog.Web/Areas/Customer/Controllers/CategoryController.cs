using FA.JustBlog.Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;

namespace FA.JustBlog.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var listCategory = unitOfWork.CategoryRepository.GetAll().ToList();
            return View(listCategory);
        }
    }
}

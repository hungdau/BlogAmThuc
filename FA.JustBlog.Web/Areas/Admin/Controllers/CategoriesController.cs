using FA.JustBlog.Common;
using FA.JustBlog.Core.Infrastructures;
using FA.JustBlog.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FA.JustBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Blog_Owner + "," + SD.Role_Contributor)]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            return View();
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //Categorie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category request)
        {
            
            if (ModelState.IsValid)
            {
                request.UrlSlug = SeoUrlHepler.FrientlyUrl(request.Name);
                this._unitOfWork.CategoryRepository.Add(request);
                _unitOfWork.SaveChange();
                TempData["success"] = "Categorie created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Categorie created error";
            return View(request);
        }

        //GET
        public IActionResult Edit(int? id)
        {

            if (id.Value == null || id.Value == 0)
            {
                return NotFound();
            }
            var CategoryFromDbFirst = _unitOfWork.CategoryRepository.GetById(id.Value);
            if (CategoryFromDbFirst == null)
            {
                return NotFound();
            }
          
            return View(CategoryFromDbFirst);
        }

        //Categorie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                obj.UrlSlug = SeoUrlHepler.FrientlyUrl(obj.Name);
                _unitOfWork.CategoryRepository.Update(obj);
                _unitOfWork.SaveChange();
                TempData["success"] = "Categorie updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Categorie created error";
            return View(obj);
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var productList = _unitOfWork.CategoryRepository.GetAll();
            return Json(new { data = productList });
        }

        //Categorie
        [HttpDelete]
        [Authorize(Roles = SD.Role_Blog_Owner)]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.CategoryRepository.GetById(id.Value);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.CategoryRepository.Delete(obj);
            _unitOfWork.SaveChange();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion

    }
}

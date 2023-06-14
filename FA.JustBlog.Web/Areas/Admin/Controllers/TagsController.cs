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
    public class TagsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //ViewBag.Abc=..
            //ViewData["Abc"]==
            return View();
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //Tag
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag request)
        {

            if (ModelState.IsValid)
            {
                request.UrlSlug = SeoUrlHepler.FrientlyUrl(request.Name);
                this._unitOfWork.TagRepository.Add(request);
                _unitOfWork.SaveChange();
                TempData["success"] = "Tag created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Tag created error";
            return View(request);
        }

        //GET
        public IActionResult Edit(int? id)
        {

            if (id.Value == null || id.Value == 0)
            {
                return NotFound();
            }
            var TagFromDbFirst = _unitOfWork.TagRepository.GetById(id.Value);
            if (TagFromDbFirst == null)
            {
                return NotFound();
            }

            return View(TagFromDbFirst);
        }

        //Tag
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tag obj)
        {

            if (ModelState.IsValid)
            {
                obj.UrlSlug = SeoUrlHepler.FrientlyUrl(obj.Name);
                _unitOfWork.TagRepository.Update(obj);
                _unitOfWork.SaveChange();
                TempData["success"] = "Tag updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Tag created error";
            return View(obj);
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var productList = _unitOfWork.TagRepository.GetAll();
            return Json(new { data = productList });
        }

        
        //Tag
        [HttpDelete]
        [Authorize(Roles = SD.Role_Blog_Owner)]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.TagRepository.GetById(id.Value);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.TagRepository.Delete(obj);
            _unitOfWork.SaveChange();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion

    }
}

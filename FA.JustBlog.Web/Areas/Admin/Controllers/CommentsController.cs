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
    public class CommentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentsController(IUnitOfWork unitOfWork)
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

        //Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Comment request)
        {

            if (ModelState.IsValid)
            {
                this._unitOfWork.CommentRepository.Add(request);
                _unitOfWork.SaveChange();
                TempData["success"] = "Comment created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Comment created error";
            return View(request);
        }

        //GET
        public IActionResult Edit(int? id)
        {

            if (id.Value == null || id.Value == 0)
            {
                return NotFound();
            }
            var CommentFromDbFirst = _unitOfWork.CommentRepository.GetById(id.Value);
            if (CommentFromDbFirst == null)
            {
                return NotFound();
            }

            return View(CommentFromDbFirst);
        }

        //Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Comment obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.CommentRepository.Update(obj);
                _unitOfWork.SaveChange();
                TempData["success"] = "Comment updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Comment created error";
            return View(obj);
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var productList = _unitOfWork.CommentRepository.GetAll(includeProperties:"Post");
            return Json(new { data = productList });
        }

        //Comment
        [HttpDelete]
        [Authorize(Roles = SD.Role_Blog_Owner)]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.CommentRepository.GetById(id.Value);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.CommentRepository.Delete(obj);
            _unitOfWork.SaveChange();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion

    }
}

using FA.JustBlog.Common;
using FA.JustBlog.Core.Infrastructures;
using FA.JustBlog.Core.Models;
using FA.JustBlog.ModelViews.PostViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FA.JustBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Blog_Owner+","+ SD.Role_Contributor)]
    public class PostsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostsController(IUnitOfWork unitOfWork)
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
            ViewBag.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            ViewBag.PublishedList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "True", Value = true.ToString(), Selected=true },
                new SelectListItem() { Text = "False", Value = false.ToString(), Selected=false },
            };
           
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Create(CreatePostVM request)
        {
			
            if (ModelState.IsValid)
            {
                //1. add tag vao database
                var tagIds = this._unitOfWork.TagRepository.AddTagByString(request.Tags);
                // 2. create postTag
                var postTags = new List<PostTagMap>();
                foreach (var tagId in tagIds)
                {
                    var postTag = new PostTagMap()
                    {
                        TagId = tagId
                    };
                    postTags.Add(postTag);
                }


                //var file = request.Image;
                var fileName = request.ImagePath.FileName; // lấy tên file
                //var FileNameNull = FileName.Remove(FileName.LastIndexOf(".")); //lấy mỗi tên file không chưa đuôi file.
                //if (ext == "" || (ext != ".jpg" && ext != ".png" && ext != ".jpeg" && ext != ".gif"))
                //{
                //    ModelState.AddModelError("", "File phải có đuôi jpg, png, gif");
                //}
                //else
                //{
                //    ModelState.AddModelError("", "File không tồn tại hoặc file phải nhỏ hơn 2MB");
                //}

                var post = new Post()
                {
                    Title = request.Title,
                    UrlSlug = SeoUrlHepler.FrientlyUrl(request.Title),
                    PostContent = request.PostContent,
                    CategoryId = request.CategoryId,
                    ShortDescription = request.ShortDescription,
                    Image = fileName,
                    PostTagMap = postTags
                };
                this._unitOfWork.PostRepository.Add(post);
                _unitOfWork.SaveChange();
                TempData["success"] = "Post created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Post created error";
            ViewBag.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            ViewBag.PublishedList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "True", Value = true.ToString(), Selected=true },
                new SelectListItem() { Text = "False", Value = false.ToString(), Selected=false },
            };
            return View(request);
        }

        //GET
        public IActionResult Edit(int? id)
        {

            if (id.Value == null || id.Value == 0)
            {
                return NotFound();
            }
            var PostFromDbFirst = _unitOfWork.PostRepository.GetById(id.Value);
            if (PostFromDbFirst == null)
            {
                return NotFound();
            }
            UpdatePostVM updatePostVM = new UpdatePostVM()
            {
                Post = PostFromDbFirst,
                CategoryList = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name", PostFromDbFirst.CategoryId),
                TagList =string.Join("; ", _unitOfWork.TagRepository.GetTagsByPost(id.Value).Select(x=>x.Name)) 
            };
            ViewBag.PublishedList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "True", Value = true.ToString(), Selected=true },
                new SelectListItem() { Text = "False", Value = false.ToString(), Selected=false },
            };
            return View(updatePostVM);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Edit(UpdatePostVM obj)
        {
           
            if (ModelState.IsValid)
            {
                //1. add tag vao database
                var tagIds = this._unitOfWork.TagRepository.AddTagByString(obj.TagList);
               //2. delete post tag
              var postTagDeletes= _unitOfWork.JustBlogContext.PostTagMap.Where(x => x.PostId == obj.Post.Id);
                foreach (var postTagMap in postTagDeletes)
                {
                    _unitOfWork.JustBlogContext.PostTagMap.Remove(postTagMap);
                }
                _unitOfWork.SaveChange();
                // 3. create postTag
                var postTags = new List<PostTagMap>();
                foreach (var tagId in tagIds)
                {
                    var postTag = new PostTagMap()
                    {
                        PostId= obj.Post.Id,
                        TagId = tagId
                    };
                    postTags.Add(postTag);
                }
                _unitOfWork.JustBlogContext.PostTagMap.AddRange(postTags);
                obj.Post.PostTagMap = postTags;
                obj.Post.UrlSlug = SeoUrlHepler.FrientlyUrl(obj.Post.Title);
                _unitOfWork.PostRepository.Update(obj.Post);
                _unitOfWork.SaveChange();
                TempData["success"] = "Post updated successfully";
                return RedirectToAction("Index");
            }
            obj.CategoryList = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name", obj.Post.CategoryId);
            TempData["error"] = "Post Edit error";
            return View(obj);
        }

      

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var productList = _unitOfWork.PostRepository.GetAll(includeProperties:"Category");
            switch (status)
            {
                case "latest":
                    productList = _unitOfWork.PostRepository.GetLatestPost(10);
                    break;
                case "mostViewed":
                    productList = _unitOfWork.PostRepository.GetMostViewedPost(10);
                    break;
                case "mostInteresting":
                    productList = _unitOfWork.PostRepository.GetHighestPosts(10);
                    break;
                case "published":
                    productList = _unitOfWork.PostRepository.GetPublisedPosts();
                    break;
                case "unPublished":
                    productList = _unitOfWork.PostRepository.GetUnpublisedPosts();
                    break;
                default:
                    break;
            }
            return Json(new { data = productList });
        }
        [Authorize(Roles = SD.Role_Blog_Owner)]
        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.PostRepository.GetById(id.Value);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.PostRepository.Delete(obj);
            _unitOfWork.SaveChange();
            return Json(new { success = true, message = "Delete Successful" });

        }
       
      
        [HttpPost]
        public JsonResult AutoComplete(string Prefix)
        {
            var names = _unitOfWork.TagRepository.GetAll().Where(p => p.Name.Contains(Prefix)).Select(p => p.Name).ToList();

            return Json(names);
        }
        #endregion

    }
}

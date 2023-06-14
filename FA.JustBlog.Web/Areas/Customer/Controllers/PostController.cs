using FA.JustBlog.Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;

namespace FA.JustBlog.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public PostController( IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var listPost = unitOfWork.PostRepository.GetAll(includeProperties: "Category").Take(6).ToList();
            //var listPost = unitOfWork.PostRepository.GetLatestPost(6);
            return View(listPost);
        }
       public IActionResult LastestPost()
        {
            return PartialView(unitOfWork.PostRepository.GetLatestPost(5));

        }
        public IActionResult Detail(int id)
        {
            var post = unitOfWork.PostRepository.GetById(id);
            return View(post);
        }
        public IActionResult Details(int year, int month, string title)
        {
            var post = unitOfWork.PostRepository.FindPost(year, month, title);
            if (post == null)
                return NotFound();
            post.ViewCount++;
            unitOfWork.PostRepository.Update(post);
            unitOfWork.SaveChange();
            return View(post);
        }
        public IActionResult GetPostByCategory(string categoryName)
        {
            var posts = unitOfWork.PostRepository.GetPostsByCategory(categoryName);
            if (posts.Count==0)
                return NotFound();
            return View(posts);
        }
        public IActionResult GetPostByTag(string tagName)
        {
            var posts = unitOfWork.PostRepository.GetPostsByTag(tagName);
            if (posts.Count == 0)
                return NotFound();
            return View(posts);
        }
        public int CountPostsForCategory(string tag)
        {
            var count = unitOfWork.PostRepository.CountPostsForTag(tag);
            return count;
        }

    }
}

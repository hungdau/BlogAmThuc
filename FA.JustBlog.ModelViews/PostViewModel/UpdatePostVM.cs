using FA.JustBlog.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FA.JustBlog.ModelViews.PostViewModel
{
    public class UpdatePostVM
    {
        public Post Post { get; set; }
        [ValidateNever]
        public SelectList CategoryList { get; set; }
        [ValidateNever]
        public string TagList { get; set; }
    }
}

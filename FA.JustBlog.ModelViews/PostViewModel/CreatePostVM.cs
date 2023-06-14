using FA.JustBlog.Core.Data;
using FA.JustBlog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FA.JustBlog.ModelViews.PostViewModel
{
    public class CreatePostVM
    {

		[Required(ErrorMessage = "Title không được để trống")]
        [StringLength(255, ErrorMessage = "Title không vượt quá 255 kí tự")]
        [TitleValid]
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string PostContent { get; set; }
        [Display(Name = "Seo Url")]
		public string Image;
		public string? UrlSlug { get; set; }
        public bool Published { get; set; }
        public DateTime? PostedOn { get; set; }
        public string Tags { get; set; }
        public int CategoryId { get; set; }
		public IFormFile ImagePath { get; set; }
	}
    public class TitleValidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
        {
            var context = (JustBlogContext)validationContext.GetService(typeof(JustBlogContext));
            if (!context.Posts.Any(a => a.Title == value.ToString()))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Title exists");
        }
    }
}

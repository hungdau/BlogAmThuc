using FA.JustBlog.Core.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.JustBlog.Core.Models
{
    public class Post
    {
     
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Post title is required.")]
        [StringLength(255)]
        //[TitleValid]
        public string Title { get; set; }

        [StringLength(1024)]
        [Column(TypeName = "ntext")]
        public string? ShortDescription { get; set; }

        [StringLength(1024)]
        [Column(TypeName = "ntext")]
        public string? PostContent { get; set; }
		public string? Image { get; set; }
		[StringLength(255)]
        public string? UrlSlug { get; set; }
        public bool? Published { get; set; }
        public DateTime? PostedOn { get; set; }
        public DateTime? Modified { get; set; } 
        public int? ViewCount { get; set; } = 0;
        public int? RateCount { get; set; } = 0;
        public int? TotalRate { get; set; } = 0;
        public decimal Rate
        {
            get
            {
                if(TotalRate == null || RateCount == null)
                    return 0;
                if (TotalRate.Value == 0 || RateCount.Value == 0)
                    return 0;
                return Convert.ToDecimal(1.0 * TotalRate.Value / RateCount.Value);
            }
        }

        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public ICollection<Comment> Comments { get; set; }
        [ValidateNever]
        public ICollection<PostTagMap> PostTagMap { get; set; }


    }
    //public class TitleValidAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value,
    //    ValidationContext validationContext)
    //    {
    //        var context = (JustBlogContext)validationContext.GetService(typeof(JustBlogContext));
    //        if (!context.Posts.Any(a => a.Title == value.ToString()))
    //        {
    //            return ValidationResult.Success;
    //        }
    //        return new ValidationResult("Title exists");
    //    }
    //}
}
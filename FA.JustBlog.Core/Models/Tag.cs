using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.JustBlog.Core.Models
{
    public class Tag
    {
      
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tag name is required.")]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? UrlSlug { get; set; }

        [StringLength(1024)]
        [Column(TypeName = "ntext")]
        public string? Description { get; set; }

        public int? Count { get; set; }
        [ValidateNever]
        public ICollection<PostTagMap> PostTagMap { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models
{
    public class PostCreateViewModel
    {
        public int PostId { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public string? Url { get; set; }
        [Display(Name ="Is Active")]
        public bool IsActive { get; set; }
        public List<Tag> Tags { get; set; } = new();
    }
}
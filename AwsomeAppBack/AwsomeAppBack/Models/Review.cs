using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwsomeAppBack.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Img { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Comment { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}

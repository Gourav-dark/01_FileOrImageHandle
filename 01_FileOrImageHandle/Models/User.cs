using System.ComponentModel.DataAnnotations;

namespace _01_FileOrImageHandle.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }


        public ICollection<Post> ?Posts { get; set; }
    }
}

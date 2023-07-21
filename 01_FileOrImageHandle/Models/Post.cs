using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace _01_FileOrImageHandle.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        [Column(TypeName = "date")]
        public DateTime PostDate { get; set; }

        //[ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Student
    {
        public int Id {  get; set; }

        public int? Average { get; set; }

        [Required]
        public int UserId  { get; set; }

        public User User { get; set; }

        [Required]
        public int SchoolClassId { get; set; }

        public SchoolClass SchoolClass { get; set; }

        public List<Result> Results { get; set; }
    }
}

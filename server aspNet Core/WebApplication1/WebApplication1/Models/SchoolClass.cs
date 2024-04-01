using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SchoolClass
    {
        public int Id { get; set; }

        [Required]
        public string ClassName { get; set; }
        public List<Student> Students { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<Exam> Exams { get; set; }
    }
}

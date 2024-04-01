using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        public string TeacherStubject {  get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } 

        public List<Exam> Exams { get; set; }

        public List<Lesson> Lessons { get; set; }
    }
}

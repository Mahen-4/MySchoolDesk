
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        public string LessonName { get; set; }

        [Required]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        [Required]
        public int SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; } 
    }
}

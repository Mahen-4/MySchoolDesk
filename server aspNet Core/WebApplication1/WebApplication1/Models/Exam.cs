using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required]
        public string ExamName { get; set; }

        [Required]
        public int SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; }

        [Required]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public List<Result> Results { get; set; }
    }
}

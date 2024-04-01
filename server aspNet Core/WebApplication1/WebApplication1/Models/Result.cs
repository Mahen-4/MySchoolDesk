using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Result
    {
        public int Id { get; set; }

        [Required]
        public float Score { get; set; }

        [Required]
        public string ResultSubject { get; set; }

        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        public int ExamId { get; set; }
        public Exam Exam {  get; set; }
    }
}

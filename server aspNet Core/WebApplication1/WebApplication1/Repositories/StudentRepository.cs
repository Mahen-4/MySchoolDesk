using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories.Repo_Interfaces;

namespace WebApplication1.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;  
        public StudentRepository(DataContext context) {
            _context = context;
        }

        public void Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }
    }
}

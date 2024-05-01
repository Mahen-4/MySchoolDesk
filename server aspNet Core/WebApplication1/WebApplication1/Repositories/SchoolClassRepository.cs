using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories.Repo_Interfaces;

namespace WebApplication1.Repositories
{
    public class SchoolClassRepository : ISchoolClassRepository
    {
        private readonly DataContext _context;
        public SchoolClassRepository(DataContext context) { 
            _context = context;
        }  

        public void Add(SchoolClass schoolClass) {
            _context.SchoolClasses.Add(schoolClass);
        }

        public void RemoveAll()
        {
            foreach (var schoolClass in _context.SchoolClasses)
            {
                _context.SchoolClasses.Remove(schoolClass);
            }
        }

        public IEnumerable<SchoolClass> GetAll() {
            return _context.SchoolClasses;
        }
    }
}

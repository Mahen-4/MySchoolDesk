using WebApplication1.Models;

namespace WebApplication1.Repositories.Repo_Interfaces
{
    public interface ISchoolClassRepository
    {

        void Add(SchoolClass schoolClass);
        void RemoveAll();
        IEnumerable<SchoolClass> GetAll();
    }
}

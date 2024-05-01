using WebApplication1.Models;

namespace WebApplication1.Repositories.Repo_Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        void RemoveAll();
    }
}

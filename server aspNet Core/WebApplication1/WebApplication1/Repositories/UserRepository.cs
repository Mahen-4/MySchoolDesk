using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories.Repo_Interfaces;

namespace WebApplication1.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;

        public UserRepository(DataContext context) {
            _context = context;
        }
        
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void RemoveAll()
        {
            foreach (var user in _context.Users)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}

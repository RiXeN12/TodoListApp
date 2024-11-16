using Library_NPR321.Repositories;
using TodoListApp.Models;

namespace TodoListApp.Repositories.AccountRepository
{
    public interface IAccountRepository : IGenericRepository<User>
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
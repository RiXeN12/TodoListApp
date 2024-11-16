using Library_NPR321.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Models;
using TodoListMain;

namespace TodoListApp.Repositories.AccountRepository
{
    public class AccountRepository
        : GenericRepository<User>, IAccountRepository
    {
        private readonly TodoDbContext _context;

        public AccountRepository(TodoDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}

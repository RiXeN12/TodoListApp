
using Microsoft.EntityFrameworkCore;
using TodoListApp.Models;
using TodoListMain;

namespace Library_NPR321.Repositories
{
    public class GenericRepository<TModel> 
        : IGenericRepository<TModel>
        where TModel : class
    {
        private readonly TodoDbContext _context;
        public GenericRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(TModel model)
        {
            await _context.AddAsync(model);
            var res = await _context.SaveChangesAsync();
            return res != 0;
        }

        protected IQueryable<TModel> GetAll()
        {
            return _context.Set<TModel>();
        }

        public async Task<bool> RemoveAsync(TModel model)
        {
            _context.Remove(model);
            var res = await _context.SaveChangesAsync();
            return res != 0;
        }

        public async Task<bool> UpdateAsync(TModel model)
        {
            _context.Update(model);
            var res = await _context.SaveChangesAsync();
            return res != 0;
        }

    }
}

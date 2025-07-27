using Microsoft.EntityFrameworkCore;
using Npgsql;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Application.Interfaces.Models;
using SeliseTaskManager.Infrastructure.Persistence;
using System.Data;
using System.Linq.Expressions;

namespace SeliseTaskManager.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        private DbSet<T> table;
        public IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_context.Database.GetConnectionString());
            }
        }

        public Repository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            table = _context.Set<T>();
        }

        public IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(_context.Database.GetConnectionString());
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await table.ToListAsync();
        }

        public int GetCount(Func<T, bool>? query)
        {
            if (query != null)
            {
                return table.Where(query).Count();
            }

            return table.Count();
        }

        public async Task<(IList<T>, int TotalCount)> Query(
            Expression<Func<T, bool>>? query, PaginationFilter paginationFilter = default!)
        {
            if (paginationFilter is null)
                paginationFilter = new PaginationFilter();

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            query ??= q => true;

            var querable = table
                    .Where(query)
                    .Take(paginationFilter.PageSize)
                    .Skip(skip);

            var count = await querable.CountAsync();
            var items = await querable.ToListAsync();

            return (items, count);
        }

        public async Task AddAsync(T obj)
        {
            await table.AddAsync(obj);
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T existing = table.Find(id) ?? default!;
            if (existing != null)
            {
                table.Remove(existing);
            }
        }
        public async Task<bool> SaveAsync()
        {
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0 ? true : false;
        }
    }
}

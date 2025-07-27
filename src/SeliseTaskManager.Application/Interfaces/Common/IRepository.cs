using System.Data;

namespace SeliseTaskManager.Application.Interfaces.Common
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T obj);
        void Update(T obj);
        void Delete(object id);
        Task<bool> SaveAsync();
        int GetCount(Func<T, bool>? query);

        //Task<IList<T>> Query(
        //    Expression<Func<T, bool>>? query,
        //    PaginationFilter paginationFilter = default!);

        IDbConnection GetDbConnection();
        Task<IEnumerable<T>> GetAll();
    }
}

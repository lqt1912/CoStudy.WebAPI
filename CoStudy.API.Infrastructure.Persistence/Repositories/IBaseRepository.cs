using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Persistence.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Add(T entity);

        long Count();

        Task<long> CountAsync();
        void Delete(ObjectId id);
        Task DeleteAsync(ObjectId id);
        bool Exists(ObjectId id);
        T Find(FilterDefinition<T> match);
        Task<T> FindAsync(FilterDefinition<T> match);

        IQueryable<T> GetAll();

        IQueryable<T> GetAll(long amount);
        Task<T> GetByIdAsync(ObjectId id);
        T GetById(ObjectId id);
        ReplaceOneResult Update(T entity, ObjectId id);
        Task<ReplaceOneResult> UpdateAsync(T entity, ObjectId id);

    }
}

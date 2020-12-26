using CoStudy.API.Infrastructure.Persistence.Contexts;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected CustomMongoClient _client;
        protected IMongoCollection<T> _collection { get; set; }

        protected BaseRepository(string alias)
        {
            _client = new CustomMongoClient();
            _collection = _client.GetDatabase().GetCollection<T>(alias);
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public void Add(T entity)
        {
            _collection.InsertOne(entity);
        }
        public long Count()
        {
            return _collection.CountDocuments(FilterDefinition<T>.Empty);
        }

        public async Task<long> CountAsync()
        {
            return await _collection.CountDocumentsAsync(FilterDefinition<T>.Empty);
        }

        public void Delete(ObjectId id)
        {
            var deleteFilter = Builders<T>.Filter.Eq("_id", id);
            _collection.DeleteOne(deleteFilter);
        }
        public async Task DeleteAsync(ObjectId id)
        {
            var deleteFilter = Builders<T>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(deleteFilter);

        }

        public bool Exists(ObjectId id)
        {
            var findFilter = Builders<T>.Filter.Eq("_id", id);
            return _collection.Find(findFilter) != null;
        }

        public T Find(FilterDefinition<T> match)
        {
            return _collection.Find(match).FirstOrDefault();
        }

        public async Task<T> FindAsync(FilterDefinition<T> match)
        {
            return await _collection.Find(match).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _collection.AsQueryable();
        }

        public IQueryable<T> GetAll(long amount)
        {
            if (Count() < amount)
                return _collection.AsQueryable();
            else return _collection.AsQueryable().Take((int)amount);
        }
        public Task<T> GetByIdAsync(ObjectId id)
        {
            var findFilter = Builders<T>.Filter.Eq("_id", id);

            return _collection.Find(findFilter).FirstOrDefaultAsync();
        }

        public T GetById(ObjectId id)
        {
            var findFilter = Builders<T>.Filter.Eq("_id", id);
            return _collection.Find(findFilter).FirstOrDefault();
        }


        public ReplaceOneResult Update(T entity, ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return _collection.ReplaceOne(filter, entity);

        }

        public Task<ReplaceOneResult> UpdateAsync(T entity, ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return _collection.ReplaceOneAsync(filter, entity);

        }

        public async Task<List<T>> FindListAsync(FilterDefinition<T> match)
        {
            return await _collection.Find(match).ToListAsync();
        }
    }
}

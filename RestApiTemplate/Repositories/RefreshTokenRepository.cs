using MongoDB.Driver;
using RestApiTemplate.Database;
using RestApiTemplate.Models;
using RestApiTemplate.Repositories.Interface;

namespace RestApiTemplate.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoCollection<RefreshToken> _collection;

        public RefreshTokenRepository(MongoDbContext context )
        {
            _collection = context.RefreshTokens;
        }
        public async Task AddAsync(RefreshToken token)
        {
            await _collection.InsertOneAsync( token );
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _collection.Find(t => t.Token == token).FirstOrDefaultAsync();
        }

        public async Task RevokeAsync(string token)
        {
            var update = Builders<RefreshToken>.Update
            .Set(t => t.IsRevoked, true)
            .Set(t => t.RevokedAt, DateTime.UtcNow);

            await _collection.UpdateOneAsync(t => t.Token == token, update);
        }
    }
}

using MongoDB.Driver;

namespace MongoDB
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }
}

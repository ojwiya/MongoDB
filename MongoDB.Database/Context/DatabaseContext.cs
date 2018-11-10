namespace MongoDB.Database
{
    public sealed class DatabaseContext : MongoContext
    {
        public DatabaseContext(string connectionString) : base(connectionString)
        {
        }
    }
}

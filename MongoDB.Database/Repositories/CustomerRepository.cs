namespace MongoDB.Database
{
    public sealed class CustomerRepository : MongoRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DatabaseContext context) : base(context)
        {
        }
    }
}

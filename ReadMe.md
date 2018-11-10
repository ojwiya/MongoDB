# MongoDB

NoSQL with MongoDB in .NET Core.

## IMongoContext

 ```csharp
public interface IMongoContext
{
    IMongoDatabase Database { get; }
}
 ```

## MongoContext

 ```csharp
public abstract class MongoContext : IMongoContext
{
    protected MongoContext(string connectionString)
    {
        Database = new MongoClient(connectionString).GetDatabase(new MongoUrl(connectionString).DatabaseName);
    }

    public IMongoDatabase Database { get; }
}
 ```

## IDocument

 ```csharp
public interface IDocument
{
    ObjectId Id { get; set; }
}
 ```

## Document

 ```csharp
public abstract class Document : IDocument
{
    [BsonExtraElements]
    public BsonDocument ExtraElements { get; set; }

    public ObjectId Id { get; set; }
}
 ```

## IRepository

 ```csharp
public interface IRepository<T> where T : class
{
    void Add(T item);

    Task AddAsync(T item);

    void AddRange(IEnumerable<T> list);

    Task AddRangeAsync(IEnumerable<T> list);

    bool Any();

    bool Any(Expression<Func<T, bool>> where);

    Task<bool> AnyAsync();

    Task<bool> AnyAsync(Expression<Func<T, bool>> where);

    long Count();

    long Count(Expression<Func<T, bool>> where);

    Task<long> CountAsync();

    Task<long> CountAsync(Expression<Func<T, bool>> where);

    void Delete(object key);

    void Delete(Expression<Func<T, bool>> where);

    Task DeleteAsync(object key);

    Task DeleteAsync(Expression<Func<T, bool>> where);

    T FirstOrDefault(Expression<Func<T, bool>> where);

    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where);

    IEnumerable<T> List();

    IEnumerable<T> List(Expression<Func<T, bool>> where);

    Task<IEnumerable<T>> ListAsync();

    Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> where);

    T Select(object key);

    Task<T> SelectAsync(object key);

    T SingleOrDefault(Expression<Func<T, bool>> where);

    Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> where);

    void Update(T item, object key);

    Task UpdateAsync(T item, object key);
}
 ```

## MongoRepository

 ```csharp
public abstract class MongoRepository<T> : IRepository<T> where T : class
{
    protected MongoRepository(IMongoContext context)
    {
        Database = context.Database;
        Collection = Database.GetCollection<T>(typeof(T).Name);
    }

    private IMongoCollection<T> Collection { get; }

    private IMongoDatabase Database { get; }

    public void Add(T item)
    {
        Collection.InsertOne(item);
    }

    public Task AddAsync(T item)
    {
        return Collection.InsertOneAsync(item);
    }

    public void AddRange(IEnumerable<T> list)
    {
        Collection.InsertMany(list);
    }

    public Task AddRangeAsync(IEnumerable<T> list)
    {
        return Collection.InsertManyAsync(list);
    }

    public bool Any()
    {
        return Collection.Find(new BsonDocument()).Any();
    }

    public bool Any(Expression<Func<T, bool>> where)
    {
        return Collection.Find(where).Any();
    }

    public Task<bool> AnyAsync()
    {
        return Collection.Find(new BsonDocument()).AnyAsync();
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> where)
    {
        return Collection.Find(where).AnyAsync();
    }

    public long Count()
    {
        return Collection.CountDocuments(new BsonDocument());
    }

    public long Count(Expression<Func<T, bool>> where)
    {
        return Collection.CountDocuments(where);
    }

    public Task<long> CountAsync()
    {
        return Collection.CountDocumentsAsync(new BsonDocument());
    }

    public Task<long> CountAsync(Expression<Func<T, bool>> where)
    {
        return Collection.CountDocumentsAsync(where);
    }

    public void Delete(object key)
    {
        Collection.DeleteOne(FilterId(key));
    }

    public void Delete(Expression<Func<T, bool>> where)
    {
        Collection.DeleteMany(where);
    }

    public Task DeleteAsync(object key)
    {
        return Collection.DeleteOneAsync(FilterId(key));
    }

    public Task DeleteAsync(Expression<Func<T, bool>> where)
    {
        return Collection.DeleteManyAsync(where);
    }

    public T FirstOrDefault(Expression<Func<T, bool>> where)
    {
        return Collection.Find(where).FirstOrDefault();
    }

    public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where)
    {
        return Collection.Find(where).FirstOrDefaultAsync();
    }

    public IEnumerable<T> List()
    {
        return Collection.Find(new BsonDocument()).ToList();
    }

    public IEnumerable<T> List(Expression<Func<T, bool>> where)
    {
        return Collection.Find(where).ToList();
    }

    public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> where)
    {
        return await Collection.Find(where).ToListAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<T>> ListAsync()
    {
        return await Collection.Find(new BsonDocument()).ToListAsync().ConfigureAwait(false);
    }

    public T Select(object key)
    {
        return Collection.Find(FilterId(key)).SingleOrDefault();
    }

    public Task<T> SelectAsync(object key)
    {
        return Collection.Find(FilterId(key)).SingleOrDefaultAsync();
    }

    public T SingleOrDefault(Expression<Func<T, bool>> where)
    {
        return Collection.Find(where).SingleOrDefault();
    }

    public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> where)
    {
        return Collection.Find(where).SingleOrDefaultAsync();
    }

    public void Update(T item, object key)
    {
        Collection.ReplaceOne(FilterId(key), item);
    }

    public Task UpdateAsync(T item, object key)
    {
        return Collection.ReplaceOneAsync(FilterId(key), item);
    }

    private static FilterDefinition<T> FilterId(object key)
    {
        return Builders<T>.Filter.Eq("Id", key);
    }
}
 ```

## DatabaseContext

 ```csharp
public sealed class DatabaseContext : MongoContext
{
    public DatabaseContext(string connectionString) : base(connectionString) { }
}
 ```

## Customer

 ```csharp
public sealed class Customer : Document
{
    public string Name { get; set; }
}
 ```

## ICustomerRepository

 ```csharp
public interface ICustomerRepository : IRepository<Customer> { }
 ```

## CustomerRepository

 ```csharp
public sealed class CustomerRepository : MongoRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(DatabaseContext context) : base(context) { }
}
 ```

## DatabaseTests

 ```csharp
[TestClass]
public class DatabaseTests
{
    public DatabaseTests()
    {
        const string connectionString = "mongodb://localhost/Database";
        var databaseContext = new DatabaseContext(connectionString);
        CustomerRepository = new CustomerRepository(databaseContext);
    }

    public ICustomerRepository CustomerRepository { get; }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryAdd()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.Select(customer.Id));
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryAddAsync()
    {
        var customer = CreateCustomer();
        CustomerRepository.AddAsync(customer);
        Assert.IsNotNull(CustomerRepository.Select(customer.Id));
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryAddRange()
    {
        var count = CustomerRepository.Count();
        CustomerRepository.AddRange(new List<Customer> { CreateCustomer() });
        Assert.IsTrue(CustomerRepository.Count() > count);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryAddRangeAsync()
    {
        var count = CustomerRepository.Count();
        CustomerRepository.AddRangeAsync(new List<Customer> { CreateCustomer() });
        Assert.IsTrue(CustomerRepository.Count() > count);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryAny()
    {
        CustomerRepository.Add(CreateCustomer());
        Assert.IsTrue(CustomerRepository.Any());
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryAnyAsync()
    {
        CustomerRepository.Add(CreateCustomer());
        Assert.IsTrue(CustomerRepository.AnyAsync().Result);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryAnyWhere()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsTrue(CustomerRepository.Any(x => x.Id == customer.Id));
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryAnyWhereAsync()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsTrue(CustomerRepository.AnyAsync(x => x.Id == customer.Id).Result);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryCount()
    {
        CustomerRepository.Add(CreateCustomer());
        Assert.IsTrue(CustomerRepository.Count() > 0);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryCountAsync()
    {
        CustomerRepository.Add(CreateCustomer());
        Assert.IsTrue(CustomerRepository.CountAsync().Result > 0);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryCountWhere()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsTrue(CustomerRepository.Count(x => x.Id == customer.Id) == 1);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryCountWhereAsync()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsTrue(CustomerRepository.CountAsync(x => x.Id == customer.Id).Result == 1);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryDelete()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.Select(customer.Id));
        CustomerRepository.Delete(customer.Id);
        Assert.IsNull(CustomerRepository.Select(customer.Id));
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryDeleteAsync()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.Select(customer.Id));
        CustomerRepository.DeleteAsync(customer.Id);
        Assert.IsNull(CustomerRepository.Select(customer.Id));
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryDeleteWhere()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.Select(customer.Id));
        CustomerRepository.Delete(x => x.Id == customer.Id);
        Assert.IsNull(CustomerRepository.Select(customer.Id));
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryDeleteWhereAsync()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.Select(customer.Id));
        CustomerRepository.DeleteAsync(x => x.Id == customer.Id);
        Assert.IsNull(CustomerRepository.Select(customer.Id));
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryFirstOrDefault()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.FirstOrDefault(x => x.Id == customer.Id));
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryFirstOrDefaultAsync()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.FirstOrDefaultAsync(x => x.Id == customer.Id).Result);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryList()
    {
        CustomerRepository.Add(CreateCustomer());
        Assert.IsTrue(CustomerRepository.List().Any());
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryListAsync()
    {
        CustomerRepository.Add(CreateCustomer());
        Assert.IsTrue(CustomerRepository.ListAsync().Result.Any());
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryListWhere()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsTrue(CustomerRepository.List(x => x.Id == customer.Id).Any());
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryListWhereAsync()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsTrue(CustomerRepository.ListAsync(x => x.Id == customer.Id).Result.Any());
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositorySelect()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.Select(customer.Id));
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositorySelectAsync()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.SelectAsync(customer.Id).Result);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositorySingleOrDefault()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.SingleOrDefault(x => x.Id == customer.Id));
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositorySingleOrDefaultAsync()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        Assert.IsNotNull(CustomerRepository.SingleOrDefaultAsync(x => x.Id == customer.Id).Result);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryUpdate()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        var oldName = customer.Name;
        customer.Name = Guid.NewGuid().ToString();
        CustomerRepository.Update(customer, customer.Id);
        customer = CustomerRepository.Select(customer.Id);
        Assert.AreNotEqual(oldName, customer.Name);
    }

    [TestMethod]
    public void DatabaseTestsCustomerRepositoryUpdateAsync()
    {
        var customer = CreateCustomer();
        CustomerRepository.Add(customer);
        var oldName = customer.Name;
        customer.Name = Guid.NewGuid().ToString();
        CustomerRepository.UpdateAsync(customer, customer.Id);
        customer = CustomerRepository.Select(customer.Id);
        Assert.AreNotEqual(oldName, customer.Name);
    }

    private static Customer CreateCustomer()
    {
        return new Customer { Id = ObjectId.GenerateNewId(), Name = Guid.NewGuid().ToString() };
    }
}
 ```

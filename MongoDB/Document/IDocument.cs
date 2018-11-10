using MongoDB.Bson;

namespace MongoDB
{
    public interface IDocument
    {
        BsonDocument ExtraElements { get; set; }

        ObjectId Id { get; set; }
    }
}

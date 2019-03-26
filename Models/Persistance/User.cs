using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AspMongo.Models.Persistance
{
    public class User
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }
}
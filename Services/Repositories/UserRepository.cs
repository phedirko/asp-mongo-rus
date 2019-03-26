using MongoDB.Driver;
using AspMongo.Models.Persistance;
using System;
using System.Collections.Generic;

namespace AspMongo.Services.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository()
        {
            _collection = new MongoClient("mongodb://localhost:27017")
                .GetDatabase("notes_db")
                .GetCollection<User>("users");
        }

        public User Insert(User user)
        {
            var existingUser = GetByUsername(user.UserName);

            if(existingUser != null)
                throw new Exception("User with same username is already exists");

            user.Id = Guid.NewGuid();
            _collection.InsertOne(user);

            return user;
        }

        public IReadOnlyCollection<User> GetAll()
        {
            return _collection
                .Find(x => true)
                .ToList();
        }

        public User GetById(Guid id)
        {
            return _collection
                .Find(x => x.Id == id)
                .FirstOrDefault();
        }

        public User GetByUsername(string username)
        {
            return _collection
                .Find(x => x.UserName == username)
                .FirstOrDefault();
        }

        public User GetByUserNameAndPassword(string username, string password)
        {
            return _collection
                .Find(
                    x => x.UserName == username &&
                         x.Password == password)
                .FirstOrDefault();
        }

        //создание индексов
        public void CreateIndexes()
        {
            _collection.Indexes.CreateOne(Builders<User>.IndexKeys.Ascending(_ => _.Id));
            _collection.Indexes.CreateOne(Builders<User>.IndexKeys.Ascending(_ => _.UserName));
        }
    }
}
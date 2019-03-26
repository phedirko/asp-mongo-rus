using MongoDB.Driver;
using AspMongo.Models.Persistance;
using System;
using System.Collections.Generic;

namespace AspMongo.Services.Repositories
{
    public class NoteRepository
    {
        private readonly IMongoCollection<Note> _collection;

        public NoteRepository()
        {
            _collection = new MongoClient("mongodb://localhost:27017")
                .GetDatabase("notes_db")
                .GetCollection<Note>("notes");
        }

        public Note Insert(Note note)
        {
            note.Id = Guid.NewGuid();
            _collection.InsertOne(note);

            return note;
        }

        public IReadOnlyCollection<Note> GetAll()
        {
            return _collection
                .Find(x => true)
                .ToList();
        }

        public Note GetById(Guid id)
        {
            return _collection
                .Find(x => x.Id == id)
                .FirstOrDefault();
        }

        public IReadOnlyCollection<Note> GetByUserId (Guid userId)
        {
            return _collection
                .Find(x => x.UserId == userId)
                .ToList();
        }

        //создание индексов
        public void CreateIndexes()
        {
            _collection.Indexes.CreateOne(Builders<Note>.IndexKeys.Ascending(_ => _.Id));
            _collection.Indexes.CreateOne(Builders<Note>.IndexKeys.Ascending(_ => _.UserId));
        }
    }
}
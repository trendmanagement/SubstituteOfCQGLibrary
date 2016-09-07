using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;

namespace RealtimeSpreadMonitor.Mongo
{
    class MongoDBConnectionAndSetup
    {
        public const string CONNECTION_STRING_NAME = "mongodb://localhost:27017";
        public const string DATABASE_NAME = "TMQRrealtime";
        public const string REALTIMEDATA_COLLECTION_NAME = "restaurants";


        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        //internal void connectToMongoDB()
        static MongoDBConnectionAndSetup()
        {
            _client = new MongoClient(CONNECTION_STRING_NAME);
            _database = _client.GetDatabase("TMQRrealtime");
        }

        public IMongoCollection<MongoDB_OptionSpreadExpression> MongoDB_OptionSpreadExpressions
        {
            get { return _database.GetCollection<MongoDB_OptionSpreadExpression>(REALTIMEDATA_COLLECTION_NAME); }
        }

        internal async Task dropCollection()
        {
            await _database.DropCollectionAsync(REALTIMEDATA_COLLECTION_NAME);
        }

        internal async Task createDoc()
        {
            MongoDB_OptionSpreadExpression osefdb = new MongoDB_OptionSpreadExpression();

            osefdb.cqgSymbol = "test";

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(osefdb);
            //Bsondo

            MongoDB.Bson.BsonDocument document
                = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(json);

            var collection = _database.GetCollection<BsonDocument>("restaurants");
            await collection.InsertOneAsync(document);
        }



        internal async Task createDocument()
        {
            var document = new BsonDocument
            {
                { "address" , new BsonDocument
                    {
                        { "street", "2 Avenue" },
                        { "zipcode", "10075" },
                        { "building", "1480" },
                        { "coord", new BsonArray { 73.9557413, 40.7720266 } }
                    }
                },
                { "borough", "Manhattan" },
                { "cuisine", "Italian" },
                { "grades", new BsonArray
                    {
                        new BsonDocument
                        {
                            { "date", new DateTime(2014, 10, 1, 0, 0, 0, DateTimeKind.Utc) },
                            { "grade", "A" },
                            { "score", 11 }
                        },
                        new BsonDocument
                        {
                            { "date", new DateTime(2014, 1, 6, 0, 0, 0, DateTimeKind.Utc) },
                            { "grade", "B" },
                            { "score", 17 }
                        }
                    }
                },
                { "name", "Vella" },
                { "restaurant_id", "41704620" }
            };

            var collection = _database.GetCollection<BsonDocument>("restaurants");
            await collection.InsertOneAsync(document);
        }

        internal async Task getDocument()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");

            //var docs = collection.Find(new BsonDocument()).ToListAsync().GetAwaiter().GetResult();

            //foreach(var x in docs)
            //{
            //    Console.WriteLine(x.cqgSymbol);
            //}


            var filter = new BsonDocument();
            var count = 0;
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        // process document
                        Console.WriteLine(document);



                        //var x = BsonSerializer.Deserialize<(document);

                        //var x = MongoDB.Bson.BsonDocument.

                        //JsonSerializer serializer = new JsonSerializer();

                        //TestExpression x = serializer.Deserialize<TestExpression>(document);

                        //Console.WriteLine(document["cqgSymbol"]);

                        count++;
                    }
                }
            }
        }
    }
    
}

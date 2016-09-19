using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeCQG.Helpers
{
    public static class ConnectionSettings
    {
        public const string ConnectionStringDefault = "mongodb://localhost:27017";
        public const string ConnectionStringTMLDB = "Data Source=h9ggwlagd1.database.windows.net;Initial Catalog = TMLDB; User ID = dataupdate; Password=6dcEpZKSFRNYk^AN;Encrypt=False;TrustServerCertificate=True";
        public const string MongoDBName = "MongoDbName";
        public const string MongoCollectionName = "MongoCollection";

        public const string QueryCollectionName = "Queries";
        public const string AnswerCollectionName = "Answers";
        public const string HandShakingCollectionName = "HandShaking";
    }
}

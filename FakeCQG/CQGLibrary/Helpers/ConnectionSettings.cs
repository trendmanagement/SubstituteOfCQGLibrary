namespace FakeCQG.Helpers
{
    public static class ConnectionSettings
    {
        public const string ConnectionStringDefault = "mongodb://localhost:27017";
        public const string MongoDBName = "MongoDbForFakeCQG";

        public const string QueryCollectionName = "Queries";
        public const string AnswerCollectionName = "Answers";
        public const string EventCollectionName = "Events";
        public const string HandshakingCollectionName = "Handshaking";
        public const string UnsubscribeHandshakingCollectionName = "UnsubscribeHandshaking";
		
		public static string ConnectionString = ConnectionStringDefault;
    }
}

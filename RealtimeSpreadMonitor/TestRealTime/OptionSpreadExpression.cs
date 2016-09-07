using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQGLibrary;

namespace TestRealTime
{
    public class OptionSpreadExpression
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Key { get; set; }


        [BsonIgnoreIfNull]
        public CQGInstrumentObj cqgInstrument;

        [BsonIgnoreIfNull]
        public FutureTimedBars futureTimedBars;

        public OptionSpreadExpression(string Id)
        {
            Key = Id;
            cqgInstrument = new CQGInstrumentObj(Id);
            futureTimedBars = new FutureTimedBars(Id);
            LoadIn(this);
        }

        public OptionSpreadExpression(string Id, CQGInstrumentObj _cqgInstrument)
        {
            Key = Id;
            cqgInstrument = _cqgInstrument;
        }

        public OptionSpreadExpression(string Id, CQGInstrumentObj _cqgInstrument, FutureTimedBars _futureTimedBars)
        {
            Key = Id;
            cqgInstrument = _cqgInstrument;
            futureTimedBars = _futureTimedBars;
            LoadIn(this);
        }

        private async void LoadIn(OptionSpreadExpression optionSpreadExpression)
        {
            try
            {
                OptionSpreadExpression model = optionSpreadExpression;
                MongoDBManager mongo = new MongoDBManager();
                var collection = mongo.GetCollectionDefault;
                await collection.InsertOneAsync(model);
                AsyncTaskListener.LogMessageFormat("Entity with Id {0} was succeed load in ", model.Key);
            }
            catch (Exception exc)
            {
                AsyncTaskListener.LogMessage(exc.Message);
            }
        }
    }
}

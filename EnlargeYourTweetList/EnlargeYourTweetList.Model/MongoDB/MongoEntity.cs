using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnlargeYourTweetList.Model.MongoDB
{
    public abstract class MongoEntity
    {
        [BsonId]
        public ObjectId ObjectId;

        public String StrId
        {
            get { return ObjectId.ToString(); }
            set { ObjectId = new ObjectId(value); }
        }
    }
}
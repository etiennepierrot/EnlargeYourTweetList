using System;
using EnlargeYourTweetList.Model.MongoDB;

namespace EnlargeYourTweetList.Model.TwitterLists
{
    public class TwitterList : MongoEntity
    {
        public long id { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
        public string description { get; set; }
        public string slug { get; set; }
        public string full_name { get; set; }
        public DateTime created_at { get; set; }
        public bool following { get; set; }

        public TwitterList()
        {
            
        }
    }
}
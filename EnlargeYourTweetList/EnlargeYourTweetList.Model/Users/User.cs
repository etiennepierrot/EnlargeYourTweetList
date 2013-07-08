using EnlargeYourTweetList.Model.MongoDB;
using EnlargeYourTweetList.Model.OAuth;

namespace EnlargeYourTweetList.Model.Users
{
    public class User : MongoEntity
    {
        public UserOAuthToken UserOAuthToken { get; set; }
    }
}
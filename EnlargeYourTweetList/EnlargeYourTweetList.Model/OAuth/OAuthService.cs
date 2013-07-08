using System.Linq;
using EnlargeYourTweetList.Model.API;
using EnlargeYourTweetList.Model.MongoDB;
using EnlargeYourTweetList.Model.Users;

namespace EnlargeYourTweetList.Model.OAuth
{
    public class OAuthService
    {
        private readonly TwitterAPI _twitterAPI;
        private readonly MongoRepository<User> _mongoRepository;

        public OAuthService(TwitterAPI twitterAPI, MongoRepository<User> mongoRepository)
        {
            _twitterAPI = twitterAPI;
            _mongoRepository = mongoRepository;
        }

        public string Authenticate()
        {
            var userToken = _twitterAPI.GetRequestToken();
            var user = new User
                {
                    UserOAuthToken = userToken
                };
            _mongoRepository.Save(user);
            return _twitterAPI.Authenticate(user.UserOAuthToken.OAuthToken);
        }

        public void GetUserOAuthToken(string oauthToken, string oauthVerifier)
        {
            var user = _mongoRepository.QueryAll().SingleOrDefault(x => x.UserOAuthToken.OAuthToken == oauthToken);
            var test = _twitterAPI.GetAccessToken(user.UserOAuthToken.OAuthToken, oauthVerifier);

        }
    }
}

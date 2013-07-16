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

        public string Authenticate(string callBackUrl)
        {
            var userToken = _twitterAPI.GetRequestToken(callBackUrl);
            var user = new User
                {
                    UserOAuthToken = userToken
                };
            _mongoRepository.Save(user);
            return _twitterAPI.Authenticate(user.UserOAuthToken.OAuthToken);
        }

        public OAuthTokenResponse GetUserOAuthToken(string oauthToken, string oauthVerifier)
        {
            var user = _mongoRepository.QueryAll().SingleOrDefault(x => x.UserOAuthToken.OAuthToken == oauthToken);
            return _twitterAPI.GetAccessToken(user.UserOAuthToken.OAuthToken, oauthVerifier);

        }
    }
}

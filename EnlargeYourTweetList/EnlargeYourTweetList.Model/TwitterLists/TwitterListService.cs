using System.Collections.Generic;
using EnlargeYourTweetList.Model.API;

namespace EnlargeYourTweetList.Model.TwitterLists
{
    public class TwitterListService
    {
        private readonly TwitterAPI _twitterAPI;

        public TwitterListService(TwitterAPI twitterAPI)
        {
            _twitterAPI = twitterAPI;
        }

        public IEnumerable<TwitterList> GetList(OAuthTokenResponse oAuthTokenResponse)
        {
            return _twitterAPI.GetProtectedRessource(oAuthTokenResponse, "1.1/lists/list.json");
        }
    }
}
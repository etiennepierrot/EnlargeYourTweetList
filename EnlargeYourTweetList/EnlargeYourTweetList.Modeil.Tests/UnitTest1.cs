using System;
using EnlargeYourTweetList.Model.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnlargeYourTweetList.Modeil.Tests
{
    [TestClass]
    public class TwitterApiTest
    {
        [TestMethod]
        public void test_the_authozation()
        {
            var twitterAPI = new TwitterAPI();
            var token = twitterAPI.GetRequestToken();
            twitterAPI.Authenticate(token.OAuthToken);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using EnlargeYourTweetList.Model.OAuth;
using EnlargeYourTweetList.Model.TwitterLists;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Contrib;


namespace EnlargeYourTweetList.Model.API
{
    public class TwitterAPI
    {
        private const string ConsumerKey = "lByCB7h9JZCv7BpFOhzIPg";
        private const string ConsumerSecret = "3e0Hp11d4iI1yka2VSrqcUvdz7yKJWEqBtH7JDBFBs";

        private const string BaseUrl = "https://api.twitter.com";

        public UserOAuthToken GetRequestToken(string callbackUrl)
        {

            var request = new RestRequest("/oauth/request_token", Method.POST);
            var restClient = new RestClient(BaseUrl)
                {
                    Authenticator = OAuth1Authenticator.ForRequestToken(
                        ConsumerKey,
                        ConsumerSecret, callbackUrl
                        )
                };
            var response = restClient.Execute(request);
            var qs = HttpUtility.ParseQueryString(response.Content);

            return new UserOAuthToken
                {
                    OAuthSecretToken = qs["oauth_token_secret"],
                    OAuthToken = qs["oauth_token"]
                };
        }

        public string Authenticate(string token)
        {
            var request = new RestRequest("/oauth/authorize?oauth_token=" + token, Method.GET);
            var response = new RestClient(BaseUrl).Execute(request);
            return response.Content;
        }

        public OAuthTokenResponse GetAccessToken(string requestToken,string verifier)
        {


            var builder = new WebRequestBuilder(
                new Uri("https://api.twitter.com/oauth/access_token"), HttpVerb.GET,new OAuthTokens { ConsumerKey = ConsumerKey, ConsumerSecret = ConsumerSecret });

            if (!string.IsNullOrEmpty(verifier))
            {
                builder.Parameters.Add("oauth_verifier", verifier);
            }

            builder.Parameters.Add("oauth_token", requestToken);

            string responseBody;

            try
            {
                HttpWebResponse webResponse = builder.ExecuteRequest();

                responseBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            }
            catch (WebException wex)
            {
                throw new Exception(wex.Message, wex);
            }

            var response = new OAuthTokenResponse
                {
                    Token = Regex.Match(responseBody, @"oauth_token=([^&]+)").Groups[1].Value,
                    TokenSecret = Regex.Match(responseBody, @"oauth_token_secret=([^&]+)").Groups[1].Value,
                    UserId = long.Parse(Regex.Match(responseBody, @"user_id=([^&]+)").Groups[1].Value,CultureInfo.CurrentCulture),
                    ScreenName = Regex.Match(responseBody, @"screen_name=([^&]+)").Groups[1].Value
                };
            return response;
        }

        public List<TwitterList> GetProtectedRessource(OAuthTokenResponse oAuthTokenResponse, string ressource)
        {
            var client = new RestClient(BaseUrl)
                {
                    Authenticator = OAuth1Authenticator.ForProtectedResource(ConsumerKey, ConsumerSecret, oAuthTokenResponse.Token,
                                                                 oAuthTokenResponse.TokenSecret)
                };

            var request = new RestRequest(ressource, Method.GET);
            var response = client.Execute<List<TwitterList>>(request);
            return response.Data;
        }


    }



}

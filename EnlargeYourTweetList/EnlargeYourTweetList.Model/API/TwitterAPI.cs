
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EnlargeYourTweetList.Model.OAuth;
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

        private RestClient GetRestClient()
        {
            return new RestClient(BaseUrl)
                {
                    Authenticator = OAuth1Authenticator.ForRequestToken(
                        ConsumerKey,
                        ConsumerSecret, "http://www.enlargeyourtweetliste.com:98/Login/CallbackUrl"
                        )
                };
        }

        public UserOAuthToken GetRequestToken()
        {

            var request = new RestRequest("/oauth/request_token", Method.POST);
            var response = GetRestClient().Execute(request);
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
            var response = GetRestClient().Execute(request);
            return response.Content;
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="requestToken">The request token.</param>
        /// <param name="verifier">The pin number or verifier string.</param>
        /// <returns>
        /// An <see cref="OAuthTokenResponse"/> class containing access token information.
        /// </returns>
        public OAuthTokenResponse GetAccessToken(string requestToken,string verifier)
        {


            WebRequestBuilder builder = new WebRequestBuilder(
                new Uri("https://api.twitter.com/oauth/access_token"),
                HttpVerb.GET,
                new OAuthTokens { ConsumerKey = ConsumerKey, ConsumerSecret = ConsumerSecret });

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

            OAuthTokenResponse response = new OAuthTokenResponse();
            response.Token = Regex.Match(responseBody, @"oauth_token=([^&]+)").Groups[1].Value;
            response.TokenSecret = Regex.Match(responseBody, @"oauth_token_secret=([^&]+)").Groups[1].Value;
            response.UserId = long.Parse(Regex.Match(responseBody, @"user_id=([^&]+)").Groups[1].Value,
                                         CultureInfo.CurrentCulture);
            response.ScreenName = Regex.Match(responseBody, @"screen_name=([^&]+)").Groups[1].Value;
            return response;
        }

        public void GetAccessToken(string token_Secret, string oauth_token, string oauth_verifier)
        {



            var client = new RestClient(BaseUrl)
                {
                    Authenticator =
                        OAuth1Authenticator.ForAccessToken(ConsumerKey, ConsumerSecret, oauth_token, oauth_verifier),
                    Proxy = new WebProxy("http://127.0.0.1:8888")
                };
            var request = new RestRequest("oauth/access_token", Method.POST);

            request.AddBody(string.Format("oauth_verifier={0}", oauth_verifier));
            request.AddHeader("Content-Type", @"application/x-www-form-urlencoded");
            request.AddHeader("Accept", "*/*");
            var response = client.Execute(request);


        }
    }



}

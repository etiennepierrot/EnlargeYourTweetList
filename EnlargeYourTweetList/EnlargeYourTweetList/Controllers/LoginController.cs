using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnlargeYourTweetList.Model.OAuth;

namespace EnlargeYourTweetList.Controllers
{
    public class LoginController : Controller
    {
        private readonly OAuthService _oAuthService;

        public LoginController(OAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        //
        // GET: /Login/

        public ActionResult Index()
        {
            var contentLogin = _oAuthService.Authenticate();
            return Content(contentLogin);
        }

        public ActionResult CallbackUrl(string oauth_token, string oauth_verifier)
        {
            _oAuthService.GetUserOAuthToken(oauth_token, oauth_verifier);
            return Content(oauth_token + "//" + oauth_verifier);
        }

    }
}

using System.Linq;
using System.Web.Mvc;
using EnlargeYourTweetList.Model.OAuth;
using EnlargeYourTweetList.Model.TwitterLists;
using EnlargeYourTweetList.Models;

namespace EnlargeYourTweetList.Controllers
{
    public class LoginController : Controller
    {
        private readonly OAuthService _oAuthService;
        private readonly TwitterListService _twitterListService;

        public LoginController(OAuthService oAuthService, TwitterListService twitterListService)
        {
            _oAuthService = oAuthService;
            _twitterListService = twitterListService;
        }

        //
        // GET: /Login/

        public ActionResult Index()
        {
            var url = string.Format("{0}PopupCallBack", HttpContext.Request.Url);
            var contentLogin = _oAuthService.Authenticate(url);
            return Content(contentLogin);
        }

        public ActionResult PopupCallBack(string oauth_token, string oauth_verifier)
        {
            var url = string.Format("CallBack?oauth_token={0}&oauth_verifier={1}", oauth_token, oauth_verifier);
            var dto = new PopupCallBackDto {url = url};
            return View(dto);
        }

        public ActionResult Callback(string oauth_token, string oauth_verifier)
        {
            var response =_oAuthService.GetUserOAuthToken(oauth_token, oauth_verifier);
            var lists =_twitterListService.GetList(response);
            var dtos = lists.Select(x => new ListDto
                {
                    Id = x.id,
                    Name = x.name
                });
            return View(dtos);
        }

    }
}

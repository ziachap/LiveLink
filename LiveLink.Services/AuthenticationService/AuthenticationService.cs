using System.Web;
using System.Web.Security;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace LiveLink.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMemberService _memberService;
        private readonly HttpContext _httpContext;

        private MembershipHelper MembershipHelper() => new MembershipHelper(UmbracoContext.Current);

        public AuthenticationService(IMemberService memberService, HttpContext httpContext)
        {
            _memberService = memberService;
            _httpContext = httpContext;
        }

        public void Logout()
        {
            _httpContext.Session.Clear();
            FormsAuthentication.SignOut();
        }

        public IMember Login(string username, string password, bool rememberMe)
        {
            if (MembershipHelper().Login(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, rememberMe);
                return _memberService.GetByUsername(username);
            }
            return null;
        }

        public IMember CurrentUser()
        {
            var currentMemberId = MembershipHelper().GetCurrentMemberId();

            if (currentMemberId < 0) return null;

            return _memberService.GetById(currentMemberId);
        }
    }
}
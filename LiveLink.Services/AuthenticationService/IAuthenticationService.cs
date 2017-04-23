using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using LiveLink.Services.Models;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace LiveLink.Services.AuthenticationService
{
	public interface IAuthenticationService
	{
		void Logout();

		IMember Login(string username, string password, bool rememberMe);

		IMember CurrentUser();
	}

	public class AuthenticationService : IAuthenticationService
	{
		private readonly IMemberService _memberService;

		private MembershipHelper MembershipHelper() => new MembershipHelper(UmbracoContext.Current);

		public AuthenticationService(IMemberService memberService)
		{
			_memberService = memberService;
		}

		public void Logout()
		{
			// TODO: Clear session
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

using System.Linq;
using System.Web.Mvc;
using Gibe.UmbracoWrappers;
using LiveLink.Services.AuthenticationService;
using LiveLink.Services.Models;
using LiveLink.Services.Models.ViewModels;
using Newtonsoft.Json;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace LiveLink.Areas.API.Controllers
{
	public class UserController : Controller
	{
		private readonly IMemberService _memberService;
		private readonly IAuthenticationService _authenticationService;
		private readonly IUmbracoWrapper _umbracoWrapper;

		public UserController(IMemberService memberService, IAuthenticationService authenticationService, IUmbracoWrapper umbracoWrapper)
		{
			_memberService = memberService;
			_authenticationService = authenticationService;
			_umbracoWrapper = umbracoWrapper;
		}

		// GET: API/User
		public string Watch(int id)
		{
			var member = _authenticationService.CurrentUser();

			if (member == null)
				return JsonConvert.SerializeObject(
					new ApiFailureResponse("Could not find current user")
				);

			var watching = (member.GetValue<string>("watching") ?? string.Empty).Split(',').ToList();
			var active = false;

			if (!watching.Contains(id.ToString()))
			{
				watching.Add(id.ToString());
				active = true;
			}
			else
			{
				watching.Remove(id.ToString());
			}
				
			member.SetValue("watching", string.Join(",", watching));
			_memberService.Save(member);

			var node = _umbracoWrapper.TypedContent(id);

			return JsonConvert.SerializeObject(
				new ApiSuccessResponse(new
				{
					Active = active,
					Title = node.Name
				})
			);
		}

		public string Login(LoginForm form)
		{
			var user = _authenticationService.Login(form.Username, form.Password, form.RememberMe);

			var response = (user != null
				? new ApiSuccessResponse(new
				{
					Name = user.Username
				})
				: (IApiResponse) new ApiFailureResponse($"User '{form.Username}' could not be authenticated"));

			return JsonConvert.SerializeObject(response);
		}
		public string Logout()
		{
			_authenticationService.Logout();
			return JsonConvert.SerializeObject(new ApiSuccessResponse(null));
		}
	}
}
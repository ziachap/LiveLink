using System.Web.Mvc;
using System.Web.Security;
using LiveLink.Services.Models.ViewModels;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace LiveLink.Controllers
{
	// Not currently in use
	public class LoginController : RenderMvcController
	{
		[HttpGet]
		public override ActionResult Index(RenderModel model)
		{
			var viewModel = new LoginViewModel(model.Content, model.CurrentCulture);
			return View("Login", viewModel);
		}


		[HttpPost]
		public ActionResult Index(LoginForm form)
		{
			// TODO: Use authentication service
			if (Membership.ValidateUser(form.Username, form.Password))
			{
				FormsAuthentication.SetAuthCookie(form.Username, form.RememberMe);
				return View();
			}

			TempData["Status"] = "Invalid Log-in Credentials";
			return View();
		}
	}
}
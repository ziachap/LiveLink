using Umbraco.Core.Models;

namespace LiveLink.Services.AuthenticationService
{
	public interface IAuthenticationService
	{
		void Logout();

		IMember Login(string username, string password, bool rememberMe);

		IMember CurrentUser();
	}
}
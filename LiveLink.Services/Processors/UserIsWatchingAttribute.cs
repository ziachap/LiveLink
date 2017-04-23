using System;
using System.Linq;
using LiveLink.Services.AuthenticationService;

namespace LiveLink.Services.Processors
{
	public class UserIsWatchingAttribute : InjectableProcessorAttribute
	{
		public Func<IAuthenticationService> AuthenticationService => Inject<IAuthenticationService>();

		public override object ProcessValue()
		{
			var user = AuthenticationService().CurrentUser();

			if (user == null) return false;

			var watching = (user.GetValue<string>("watching") ?? string.Empty).Split(',').ToList();

			return watching.Contains(Context.Content.Id.ToString());
		}
	}
}

using System;
using System.Web.Mvc;
using Umbraco.Web;

namespace LiveLink
{
	public class Global : UmbracoApplication
	{
		public Global()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		void Application_Start(object sender, EventArgs e)
		{
			base.Application_Start(sender, e);

			AreaRegistration.RegisterAllAreas();
		}
	}
}
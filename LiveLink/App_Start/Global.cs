using System;
using System.Web.Mvc;
using StackExchange.Profiling;
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

		protected void Application_BeginRequest()
		{
			if (Request.IsLocal)
			{
				MiniProfiler.Start();
			}
		}

		protected void Application_EndRequest()
		{
			MiniProfiler.Stop();
		}
	}
}
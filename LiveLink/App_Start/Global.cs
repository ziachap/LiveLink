using System;
using System.Web.Mvc;
using Examine;
using Examine.LuceneEngine.Providers;
using LiveLink.App_Start;
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

			var eventIndexer = (LuceneIndexer)ExamineManager.Instance.IndexProviderCollection["EventIndexer"];
			eventIndexer.DocumentWriting += ExamineIndexer.EventIndexer_DocumentWriting;

			AreaRegistration.RegisterAllAreas();

		}

		protected void Application_BeginRequest()
		{
			if (Request.IsLocal)
			{
				//MiniProfiler.Start();
			}
		}

		protected void Application_EndRequest()
		{
			//MiniProfiler.Stop();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gibe.DittoProcessors.Processors;
using Gibe.DittoServices.ModelConverters;
using Gibe.UmbracoWrappers;
using LiveLink.Services.EventSearchService;
using LiveLink.Services.Models.ViewModels;

namespace LiveLink.Services.Processors
{
	public class VenueEventsAttribute : InjectableProcessorAttribute
	{
		public Func<IEventSearchService> EventSearchService => Inject<IEventSearchService>();

		public override object ProcessValue()
		{
			return EventSearchService().GetVenueEvents(new GetEventsConfiguration());
		}
	}
}

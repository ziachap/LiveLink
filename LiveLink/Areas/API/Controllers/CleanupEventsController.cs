using System;
using System.Linq;
using System.Web.Mvc;
using LiveLink.Services.EventSearchService;
using Umbraco.Core.Services;

namespace LiveLink.Areas.API
{
	public class CleanupEventsController : Controller
	{
		private readonly IEventSearchService _eventSearchService;
		private readonly IContentService _contentService;

		public CleanupEventsController(IEventSearchService eventSearchService, IContentService contentService)
		{
			_eventSearchService = eventSearchService;
			_contentService = contentService;
		}

		// GET: API/CleanupEvents
		public object Index()
		{
			var results = _eventSearchService.GetVenueEvents(new GetEventsConfiguration
			{
				EarliestDate = DateTime.MinValue,
				LatestDate = DateTime.Now.AddDays(-3)
			});

			foreach (var publishedContent in results)
			{
				var content = _contentService.GetById(publishedContent.Id);
				_contentService.Delete(content);
			}

			// TODO: Generic API Response type

			//return JsonConvert.SerializeObject(new ApiSuccessResponse(results));
			return results.Select(x => x.Name);
		}
	}
}
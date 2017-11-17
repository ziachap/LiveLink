using System;
using System.Linq;
using System.Web.Mvc;
using Gibe.UmbracoWrappers;
using LiveLink.Services.EventSearchService;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace LiveLink.Areas.API
{
	public class CleanupEventsController : Controller
	{
		private readonly IEventSearchService _eventSearchService;
		private readonly IContentService _contentService;
		private readonly IMediaService _mediaService;

		public CleanupEventsController(IEventSearchService eventSearchService,
			IContentService contentService, 
			IMediaService mediaService)
		{
			_eventSearchService = eventSearchService;
			_contentService = contentService;
			_mediaService = mediaService;
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
				RemoveEventMedia(content);
				_contentService.Delete(content);
			}

			// TODO: Generic API Response type

			//return JsonConvert.SerializeObject(new ApiSuccessResponse(results));
			return results.Select(x => x.Name);
		}

		private void RemoveEventMedia(IContent content)
		{
			var mediaId = content.GetValue<int>("contentThumbnail");
			var media = _mediaService.GetById(mediaId);
			_mediaService.Delete(media);
		}
	}
}
using System;
using System.Linq;
using System.Web.Mvc;
using Examine;
using Gibe.UmbracoWrappers;
using log4net;
using LiveLink.Services.DateTimeProvider;
using LiveLink.Services.DuplicatesService;
using LiveLink.Services.EventSearchService;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace LiveLink.Areas.API
{
	public class CleanupEventsController : Controller
	{
		private readonly IContentService _contentService;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly IDuplicatesService _duplicatesService;
		private readonly IEventSearchService _eventSearchService;
		private readonly ILog _log;
		private readonly IMediaService _mediaService;
		private readonly IUmbracoWrapper _umbracoWrapper;

		public CleanupEventsController(IEventSearchService eventSearchService,
			IContentService contentService,
			IMediaService mediaService,
			IDuplicatesService duplicatesService,
			ILog log,
			IUmbracoWrapper umbracoWrapper,
			IDateTimeProvider dateTimeProvider)
		{
			_eventSearchService = eventSearchService;
			_contentService = contentService;
			_mediaService = mediaService;
			_duplicatesService = duplicatesService;
			_log = log;
			_umbracoWrapper = umbracoWrapper;
			_dateTimeProvider = dateTimeProvider;
		}

		private void Log(string message)
		{
			_log.Debug("[CLEANUP EVENTS] " + message);
		}

		public object Debug()
		{
			Log($"START: Gathering events to cleanup");

			var config = new GetEventsConfiguration
			{
				EarliestDate = DateTime.MinValue,
				LatestDate = _dateTimeProvider.Now().AddDays(-3)
			};

			Log(
				$"Config: {config.EarliestDate.Value.ToString("yyyy MMMM dd")} -> {config.LatestDate.Value.ToString("yyyy MMMM dd")}");

			var results = _eventSearchService.GetVenueEvents(config);

			if (!results.Any())
			{
				Log($"NO RESULTS FOUND");
			}

			foreach (var publishedContent in results)
			{
				var startDate = _umbracoWrapper.GetPropertyValue<DateTime>(publishedContent, "contentStartDateTime");
				if (startDate > _dateTimeProvider.Now().AddDays(-2))
				{
					Log(
						$"Event is NOT in cleanup date range! {publishedContent.Id}|{publishedContent.Name}|STARTS:{startDate.ToString("yyyy MMMM dd")}");
				}
				else
				{
					Log($"Event is in cleanup date range {publishedContent.Id}|{publishedContent.Name}");
				}
			}

			Log($"COMPLETE: Gathering events to cleanup");

			return "OK";
		}

		// GET: API/CleanupEvents
		public object Index()
		{
			var results = _eventSearchService.GetVenueEvents(new GetEventsConfiguration
			{
				EarliestDate = DateTime.MinValue,
				LatestDate = _dateTimeProvider.Now().AddDays(-3)
			});

			foreach (var publishedContent in results)
			{
				var startDate = _umbracoWrapper.GetPropertyValue<DateTime>(publishedContent, "contentStartDateTime");
				Log($"Deleting event: {publishedContent.Id}|{publishedContent.Name}|{startDate:yyyy MMMM dd}");

				var content = _contentService.GetById(publishedContent.Id);
				RemoveEventMedia(content);

				_contentService.Delete(content);
			}

			_duplicatesService.RemoveDuplicates();

			ExamineManager.Instance.RebuildIndex();

			// TODO: Make some generic API Response type
			//return JsonConvert.SerializeObject(new ApiSuccessResponse(results));
			return results.Count();
		}

		private void RemoveEventMedia(IContent content)
		{
			var mediaId = content.GetValue<int?>("contentThumbnail");
			if (mediaId.HasValue)
			{
				var media = _mediaService.GetById(mediaId.Value);
				if (media != null)
				{
					_mediaService.Delete(media);
				}
			}
		}
	}
}
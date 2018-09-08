using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.UmbracoWrappers;
using log4net;
using LiveLink.Services.EventImportService;
using LiveLink.Services.FacebookEventsService.Calls;
using LiveLink.Services.Models;
using Skybrud.Social.Facebook.Options.Events;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace LiveLink.Services.FacebookEventsService
{
	public class FacebookEventsService : IFacebookEventsService
	{
		private readonly IFacebookApiCall<GetEventsConfiguration, GetEventsResponse> _getEventsCall;
		private readonly ILog _log;
		private readonly IUmbracoImageRetriever _umbracoImageRetriever;
		private readonly IUmbracoWrapper _umbracoWrapper;


		public FacebookEventsService(IFacebookApiCall<GetEventsConfiguration, GetEventsResponse> getEventsCall,
			IUmbracoWrapper umbracoWrapper,
			ILog log,
			IUmbracoImageRetriever umbracoImageRetriever)
		{
			_getEventsCall = getEventsCall;
			_umbracoWrapper = umbracoWrapper;
			_log = log;
			_umbracoImageRetriever = umbracoImageRetriever;
		}

		// The default here might cause confusion in the future
		// Perhaps remove nullability from limit?
		public IEnumerable<LiveLinkEvent> GetEventsForVenues(int? limit = 10)
		{
			return Venues().SelectMany(x => GetEventsForVenue(x, limit)).ToList();
		}

		public IEnumerable<LiveLinkEvent> GetEventsForVenue(IPublishedContent venueContent, int? limit)
		{
			var config = new GetEventsConfiguration
			{
				Identifier = _umbracoWrapper
					.GetPropertyValue<string>(venueContent, "developerFacebookPageIdentifier"),
				EventsConfiguration = new FacebookEventsOptions
				{
					Limit = limit
				}
			};

			var response = _getEventsCall.Call(config);

			return AsLiveLinkEvents(response.Events, venueContent.Id);
		}

		private IEnumerable<LiveLinkEvent> AsLiveLinkEvents(IEnumerable<FacebookEvent> facebookEvents, int venueNodeId)
		{
			foreach (var facebookEvent in facebookEvents)
			{
				LiveLinkEvent liveLinkEvent = null;
				try
				{
					liveLinkEvent = new LiveLinkEvent
					{
						Title = facebookEvent.Name,
						Description = facebookEvent.Description,
						StartDateTime = facebookEvent.StartDateTime,
						EndDateTime = facebookEvent.EndDateTime,
						VenueNodeId = venueNodeId,
						FacebookEventIdentifier = facebookEvent.Id,
						TicketUri = facebookEvent.TicketUri,
						Thumbnail = _umbracoImageRetriever.RetrieveAndSaveImage(facebookEvent.CoverUrl,
							facebookEvent.Id)
					};
				}
				catch (Exception ex)
				{
					_log.Error("Could not convert Facebook event to LiveLink event", ex);
				}

				if (liveLinkEvent != null)
				{
					yield return liveLinkEvent;
				}
			}
		}

		private IPublishedContent Settings()
		{
			return _umbracoWrapper.TypedContentAtRoot().First(x => x.DocumentTypeAlias.Equals("settings"));
		}

		private IEnumerable<IPublishedContent> Venues()
		{
			return Settings().Children.First(x => x.DocumentTypeAlias.Equals("locations"))
				.Descendants().Where(x => x.DocumentTypeAlias.Equals("venue"));
		}
	}
}
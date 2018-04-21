using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.UmbracoWrappers;
using log4net;
using LiveLink.Services.EventImportService;
using LiveLink.Services.Models;
using Skybrud.Social.Facebook.Options.Events;
using Skybrud.Social.Umbraco.Facebook.PropertyEditors.OAuth;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace LiveLink.Services.FacebookEventsService
{
	public interface IFacebookEventsService
	{
		IEnumerable<LiveLinkEvent> GetEventsForVenues(int? limit = 10);
		IEnumerable<LiveLinkEvent> GetEventsForVenue(IPublishedContent venueContent, int? limit);
	}

	public class FacebookEventsService : IFacebookEventsService
	{
		private readonly IFacebookApiWrapper _facebookApiWrapper;
		private readonly IUmbracoWrapper _umbracoWrapper;
		private readonly ILog _log;
		private readonly IUmbracoImageRetriever _umbracoImageRetriever;


		public FacebookEventsService(IFacebookApiWrapper facebookApiWrapper, IUmbracoWrapper umbracoWrapper, ILog log, IUmbracoImageRetriever umbracoImageRetriever)
		{
			_facebookApiWrapper = facebookApiWrapper;
			_umbracoWrapper = umbracoWrapper;
			_log = log;
			_umbracoImageRetriever = umbracoImageRetriever;
		}

		public IEnumerable<LiveLinkEvent> GetEventsForVenues(int? limit = 10)
		{
			var eventsOptions = new FacebookEventsOptions
			{
				Limit = limit
			};

			return Venues().SelectMany(x => GetEventsForVenue(x, AuthData(), eventsOptions)).ToList();
		}

		public IEnumerable<LiveLinkEvent> GetEventsForVenue(IPublishedContent venueContent, int? limit)
		{
			var eventsOptions = new FacebookEventsOptions
			{
				Limit = limit
			};

			return GetEventsForVenue(venueContent, AuthData(), eventsOptions);
		}

		private IEnumerable<LiveLinkEvent> GetEventsForVenue(IPublishedContent venue,
			FacebookOAuthData authData, FacebookEventsOptions options)
		{
			var pageId = _umbracoWrapper.GetPropertyValue<string>(venue,
				"developerFacebookPageIdentifier");

			var events = _facebookApiWrapper.GetEvents(authData, options, pageId);

			return AsLiveLinkEvents(events, venue.Id);
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
						Thumbnail = _umbracoImageRetriever.RetrieveAndSaveImage(facebookEvent.CoverUrl, facebookEvent.Id)
					};
				}
				catch (Exception ex)
				{
					_log.Error("Could not convert Facebook event to LiveLink event", ex);
				}

				if (liveLinkEvent != null) yield return liveLinkEvent;
			}
		}

		private FacebookOAuthData AuthData()
			=> _umbracoWrapper.GetPropertyValue<FacebookOAuthData>(Settings(), "settingsFacebookOAuth");

		private IPublishedContent Settings()
			=> _umbracoWrapper.TypedContentAtRoot().First(x => x.DocumentTypeAlias.Equals("settings"));

		private IEnumerable<IPublishedContent> Venues()
			=> Settings().Children.First(x => x.DocumentTypeAlias.Equals("locations"))
				.Descendants().Where(x => x.DocumentTypeAlias.Equals("venue"));
	}
}
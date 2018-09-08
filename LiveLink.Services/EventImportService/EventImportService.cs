using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.UmbracoWrappers;
using LiveLink.Services.DateTimeProvider;
using LiveLink.Services.Models;
using LiveLink.Services.TagService;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace LiveLink.Services.EventImportService
{
	public class EventImportService : IEventImportService
	{
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ISmartTagService _smartTagService;
		private readonly IUmbracoWrapper _umbracoWrapper;

		// TODO: This is facebook related and shouldn't be in this class
		private IDictionary<string, int> _eventIdentifierNodeMap;

		public EventImportService(IUmbracoWrapper umbracoWrapper,
			ISmartTagService smartTagService,
			IDateTimeProvider dateTimeProvider)
		{
			_umbracoWrapper = umbracoWrapper;
			_smartTagService = smartTagService;
			_dateTimeProvider = dateTimeProvider;
		}

		public void SaveEvents(IEnumerable<LiveLinkEvent> events)
		{
			var futureEvents = events.Where(x => x.StartDateTime > _dateTimeProvider.Now()).ToList();

			var contentService = ContentService();

			_eventIdentifierNodeMap = CreateEventIdentifierNodeMap(futureEvents);

			foreach (var liveLinkEvent in futureEvents)
			{
				if (_eventIdentifierNodeMap.ContainsKey(liveLinkEvent.FacebookEventIdentifier))
				{
					var eventContent =
						contentService.GetById(_eventIdentifierNodeMap[liveLinkEvent.FacebookEventIdentifier]);
					UpdateAndSaveEvent(contentService, eventContent, liveLinkEvent);
				}
				else
				{
					var venueContent = contentService.GetById(liveLinkEvent.VenueNodeId);
					var eventContent =
						contentService.CreateContentWithIdentity(liveLinkEvent.Title, venueContent.Id, "event");
					UpdateAndSaveEvent(contentService, eventContent, liveLinkEvent);
				}
			}
		}

		// TODO: Surely this can be injected?
		private IContentService ContentService()
		{
			return ApplicationContext.Current.Services.ContentService;
		}

		private void UpdateAndSaveEvent(IContentService contentService, IContent eventContent,
			LiveLinkEvent liveLinkEvent)
		{
			eventContent.SetValue("contentTitle", liveLinkEvent.Title);
			eventContent.SetValue("contentSummary", liveLinkEvent.Description);
			eventContent.SetValue("contentDescription", FormatAsHtml(liveLinkEvent.Description));
			eventContent.SetValue("contentStartDateTime", liveLinkEvent.StartDateTime);
			eventContent.SetValue("contentEndDateTime", liveLinkEvent.EndDateTime);
			eventContent.SetValue("contentTicketURI", liveLinkEvent.TicketUri);
			eventContent.SetValue("contentThumbnail", liveLinkEvent.Thumbnail);
			eventContent.SetValue("contentTags", ExtractTags(liveLinkEvent.Description));
			eventContent.SetValue("developerFacebookEventIdentifier", liveLinkEvent.FacebookEventIdentifier);

			eventContent.SetValue("metaTitle", liveLinkEvent.Title);
			eventContent.SetValue("metaDescription", liveLinkEvent.Description);

			contentService.SaveAndPublishWithStatus(eventContent);
		}

		private IDictionary<string, int> CreateEventIdentifierNodeMap(IEnumerable<LiveLinkEvent> events)
		{
			var dictionary = new Dictionary<string, int>();

			var eventNodes = ContentService().GetByIds(Venues().Select(x => x.Id))
				.SelectMany(x => x.Children());

			var existingEventsMap = ExistingEventMap(eventNodes);

			foreach (var existingEvent in existingEventsMap)
			{
				if (events.Any(x => existingEvent.Item1.Equals(x.FacebookEventIdentifier)))
				{
					if (!dictionary.ContainsKey(existingEvent.Item1))
					{
						dictionary.Add(existingEvent.Item1, existingEvent.Item2);
					}
				}
			}

			return dictionary;
		}

		private IEnumerable<Tuple<string, int>> ExistingEventMap(IEnumerable<IContent> events)
		{
			var existingEventMap = new List<Tuple<string, int>>();
			foreach (var eventNode in events)
			{
				var identifier = eventNode.GetValue<string>("developerFacebookEventIdentifier");

				if (!string.IsNullOrEmpty(identifier))
				{
					existingEventMap.Add(new Tuple<string, int>(identifier, eventNode.Id));
				}
			}

			return existingEventMap;
		}

		private string FormatAsHtml(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}

			var paragraphedText = "<p>" + text.Replace("\n", "<br />") + "</p>";

			return paragraphedText;
		}

		private string ExtractTags(string text)
		{
			return string.Join(",", _smartTagService.ExtractTags(text));
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
using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.UmbracoWrappers;
using LiveLink.Services.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace LiveLink.Services.EventImportService
{
	public class EventImportService : IEventImportService
	{
		private readonly IUmbracoWrapper _umbracoWrapper;

		private IContentService ContentService() 
			=> ApplicationContext.Current.Services.ContentService;

		// TODO: This is facebook related and should probably go somewhere else
		private IDictionary<string, int> EventIdentifierNodeMap;

		public EventImportService(IUmbracoWrapper umbracoWrapper)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		public void SaveEvents(IEnumerable<LiveLinkEvent> events)
		{
			var futureEvents = events.Where(x => x.StartDateTime > DateTime.Now).ToList();

			var contentService = ContentService();

			EventIdentifierNodeMap = CreateEventIdentifierNodeMap(futureEvents);

			foreach (var liveLinkEvent in futureEvents)
			{
				if (EventIdentifierNodeMap.ContainsKey(liveLinkEvent.FacebookEventIdentifier))
				{
					var eventContent = contentService.GetById(EventIdentifierNodeMap[liveLinkEvent.FacebookEventIdentifier]);
					UpdateAndSaveEvent(contentService, eventContent, liveLinkEvent);
				}
				else
				{
					var venueContent = contentService.GetById(liveLinkEvent.VenueNodeId);
					var eventContent = contentService.CreateContentWithIdentity(liveLinkEvent.Title, venueContent.Id, "event");
					UpdateAndSaveEvent(contentService, eventContent, liveLinkEvent);
				}
			}
		}
        
		private void UpdateAndSaveEvent(IContentService contentService, IContent eventContent, LiveLinkEvent liveLinkEvent)
		{
			eventContent.SetValue("contentTitle", liveLinkEvent.Title);
			eventContent.SetValue("contentSummary", liveLinkEvent.Description);
			eventContent.SetValue("contentDescription", FormatAsHtml(liveLinkEvent.Description));
			eventContent.SetValue("contentStartDateTime", liveLinkEvent.StartDateTime);
			eventContent.SetValue("contentEndDateTime", liveLinkEvent.EndDateTime);
			eventContent.SetValue("contentTicketURI", liveLinkEvent.TicketUri);
			eventContent.SetValue("contentThumbnail", liveLinkEvent.Thumbnail);
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
					dictionary.Add(existingEvent.Item1, existingEvent.Item2);
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
					existingEventMap.Add(new Tuple<string, int>(identifier, eventNode.Id));
			}

			return existingEventMap;
		}
		
		private string FormatAsHtml(string text)
		{
			if (string.IsNullOrEmpty(text)) return text;

			var paragraphedText = "<p>" + text.Replace("\n", "<br />") + "</p>";

			return paragraphedText;
		}

		private IPublishedContent Settings()
			=> _umbracoWrapper.TypedContentAtRoot().First(x => x.DocumentTypeAlias.Equals("settings"));

		private IEnumerable<IPublishedContent> Venues()
		   => Settings().Children.First(x => x.DocumentTypeAlias.Equals("locations"))
		   .Descendants().Where(x => x.DocumentTypeAlias.Equals("venue"));

	}
}
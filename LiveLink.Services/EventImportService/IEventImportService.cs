using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.DittoServices;
using Gibe.UmbracoWrappers;
using LiveLink.Services.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace LiveLink.Services.EventImportService
{
    public interface IEventImportService
    {
        void SaveEvents(IEnumerable<LiveLinkEvent> events);
    }

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
            var contentService = ContentService();

            EventIdentifierNodeMap = CreateEventIdentifierNodeMap(events);

            foreach (var liveLinkEvent in events)
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

        // TODO: This might want to go in a dedicated service
        private string FormatAsHtml(string text)
        {
            var paragraphedText = "<p>" + string.Join("</p><p>", text.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries)) + "</p>";

            return paragraphedText;
        }

        private IPublishedContent Settings()
            => _umbracoWrapper.TypedContentAtRoot().First(x => x.DocumentTypeAlias.Equals("settings"));

        private IEnumerable<IPublishedContent> Venues()
            => Settings().Children.First(x => x.DocumentTypeAlias.Equals("venues")).Children;

    }
}

using System.Collections.Generic;
using System.Linq;
using Gibe.UmbracoWrappers;
using LiveLink.Services.Models;
using Skybrud.Social.Facebook.Options.Events;
using Skybrud.Social.Umbraco.Facebook.PropertyEditors.OAuth;
using Umbraco.Core.Models;

namespace LiveLink.Services.FacebookEventsService
{
    public interface IFacebookEventsService
    {
        IEnumerable<LiveLinkEvent> GetEventsForVenues();
    }

    public class FacebookEventsService : IFacebookEventsService
    {
        private readonly IFacebookApiWrapper _facebookApiWrapper;
        private readonly IUmbracoWrapper _umbracoWrapper;
        

        public FacebookEventsService(IFacebookApiWrapper facebookApiWrapper, IUmbracoWrapper umbracoWrapper)
        {
            _facebookApiWrapper = facebookApiWrapper;
            _umbracoWrapper = umbracoWrapper;
        }

        public IEnumerable<LiveLinkEvent> GetEventsForVenues()
        {
            var authData = _umbracoWrapper.GetPropertyValue<FacebookOAuthData>(Settings(),
                "settingsFacebookOAuth");

            var eventsOptions = new FacebookEventsOptions
            {
                Limit = 10
            };

            return Venues().SelectMany(x => GetEventsForVenue(x, authData, eventsOptions)).ToList();
        }

        public IEnumerable<LiveLinkEvent> GetEventsForVenue(IPublishedContent venue,
            FacebookOAuthData authData, FacebookEventsOptions options)
        {
            var pageId = _umbracoWrapper.GetPropertyValue<string>(venue,
                "developerFacebookPageIdentifier");

            var events = _facebookApiWrapper.GetEvents(authData, options, pageId);
            
            return events.Select(x => ToLiveLinkEvent(x, venue.Id));
        }

        private LiveLinkEvent ToLiveLinkEvent(FacebookEvent facebookEvent, int venueNodeId)
        {
            return new LiveLinkEvent
            {
                Title = facebookEvent.Name,
                Description = facebookEvent.Description,
                StartDateTime = facebookEvent.StartDateTime,
                EndDateTime = facebookEvent.EndDateTime,
                VenueNodeId = venueNodeId,
                FacebookEventIdentifier = facebookEvent.Id,
                TicketUri = facebookEvent.TicketUri
            };
        }

        private IPublishedContent Settings()
            => _umbracoWrapper.TypedContentAtRoot().First(x => x.DocumentTypeAlias.Equals("settings"));

        private IEnumerable<IPublishedContent> Venues()
            => Settings().Children.First(x => x.DocumentTypeAlias.Equals("venues")).Children;
    }
}
using System.Collections.Generic;
using Skybrud.Social.Facebook.Options.Events;
using Skybrud.Social.Umbraco.Facebook.PropertyEditors.OAuth;
using SkybrudFacebookService = Skybrud.Social.Facebook.FacebookService;

namespace LiveLink.Services.FacebookEventsService
{
    public interface IFacebookApiWrapper
    {
        IEnumerable<FacebookEvent> GetEvents(FacebookOAuthData authenticationData,
            FacebookEventsOptions eventsConfiguration, string identifier);

        SkybrudFacebookService Service(FacebookOAuthData authenticationData);
    }
}
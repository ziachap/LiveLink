using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.Social.Facebook;
using Skybrud.Social.Facebook.Options.Events;
using Skybrud.Social.Json;
using Skybrud.Social.Umbraco.Facebook.PropertyEditors.OAuth;
using Umbraco.Core.Logging;

namespace LiveLink.Services.FacebookEventsService
{
    public class FacebookApiWrapper : IFacebookApiWrapper
    {
		// TODO: This belongs in IFacebookEventsService
        private readonly string[] _fields = {
            "id",
            "name",
            "description",
            "start_time",
            "end_time",
            "cover",
            "ticket_uri",
			"place"
            };

        public IEnumerable<FacebookEvent> GetEvents(FacebookOAuthData authenticationData,
            FacebookEventsOptions eventsConfiguration, string identifier)
        {
            try
            {
                var jsonData = Service(authenticationData).Client
                    .DoAuthenticatedGetRequest("/" + identifier + $"/events?fields={string.Join(",", _fields)}&limit=10")
                    .GetBodyAsJsonObject().GetArray("data");
                return AsFacebookEvents(jsonData);
            }
            catch (Exception e)
            {
                LogHelper.Error<FacebookApiWrapper>("Unable to retrieve events from facebook", e);
                return Enumerable.Empty<FacebookEvent>();
            }
        }

        public FacebookService Service(FacebookOAuthData authenticationData)
            => FacebookService.CreateFromAccessToken(authenticationData.AccessToken);

        private IEnumerable<FacebookEvent> AsFacebookEvents(JsonArray facebookEvents)
        {
            for (int i = 0; i < facebookEvents.Length; i++)
            {
                yield return FacebookEvent((Dictionary<string, object>) facebookEvents[i]);
            }
        }

        private FacebookEvent FacebookEvent(Dictionary<string, object> properties)
        {
	        var cover = GetValueOrDefault(properties, "cover") as Dictionary<string, object>;

			// TODO: Can pull venues from this
			//var place = GetValueOrDefault(properties, "place") as Dictionary<string, object>;
			
			return new FacebookEvent
            {
                Id = GetValueOrDefault(properties, "id") as string,
                Name = GetValueOrDefault(properties, "name") as string,
                StartDateTime = GetDateTimeOrDefault(properties, "start_time"),
                EndDateTime = GetDateTimeOrDefault(properties, "end_time"),
                Description = GetValueOrDefault(properties, "description") as string,
                TicketUri = GetValueOrDefault(properties, "ticket_uri") as string,
				CoverUrl = GetValueOrDefault(cover, "source") as string,
			};
        }

	    private DateTime? GetDateTimeOrDefault(IDictionary<string, object> dictionary, string key)
	    {
			var dateTimeText = GetValueOrDefault(dictionary, key) as string;
			DateTime? dateTime = null;
			if (!string.IsNullOrEmpty(dateTimeText))
			{
				dateTime = DateTime.Parse(dateTimeText);
			}
		    return dateTime;
	    }


		private object GetValueOrDefault(IDictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            return null;
        }
    }
}
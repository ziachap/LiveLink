using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.Social.Facebook.Options.Events;
using Skybrud.Social.Json;

namespace LiveLink.Services.FacebookEventsService.Calls
{
	public class GetEventsCall : IFacebookApiCall<GetEventsConfiguration, GetEventsResponse>
	{
		private readonly IFacebookApiWrapper _facebookApiWrapper;

		private readonly string[] _fields =
		{
			"id",
			"name",
			"description",
			"start_time",
			"end_time",
			"cover",
			"ticket_uri",
			"place"
		};

		public GetEventsCall(IFacebookApiWrapper facebookApiWrapper)
		{
			_facebookApiWrapper = facebookApiWrapper;
		}

		public GetEventsResponse Call(GetEventsConfiguration config)
		{
			var url = $"/{config.Identifier}/events?fields={string.Join(",", _fields)}&limit=10";

			// TODO: Work out why Events() on the client doesn't work
			// This is a temporary workaround which directly uses the api url
			var jsonData = _facebookApiWrapper.Execute(c => 
				c.Client.DoAuthenticatedGetRequest(url).GetBodyAsJsonObject().GetArray("data"));

			return jsonData == null 
				? new GetEventsResponse(Enumerable.Empty<FacebookEvent>()) 
				: new GetEventsResponse(AsFacebookEvents(jsonData).ToList());
		}

		private IEnumerable<FacebookEvent> AsFacebookEvents(JsonArray facebookEvents)
		{
			for (var i = 0; i < facebookEvents.Length; i++)
			{
				yield return FacebookEvent((Dictionary<string, object>) facebookEvents[i]);
			}
		}

		private FacebookEvent FacebookEvent(Dictionary<string, object> properties)
		{
			var cover = GetValueOrDefault(properties, "cover") as Dictionary<string, object>;

			// TODO: Can pull venues from this eventually, for promoter pages
			//var place = GetValueOrDefault(properties, "place") as Dictionary<string, object>;

			return new FacebookEvent
			{
				Id = GetValueOrDefault(properties, "id") as string,
				Name = GetValueOrDefault(properties, "name") as string,
				StartDateTime = GetDateTimeOrDefault(properties, "start_time"),
				EndDateTime = GetDateTimeOrDefault(properties, "end_time"),
				Description = GetValueOrDefault(properties, "description") as string,
				TicketUri = GetValueOrDefault(properties, "ticket_uri") as string,
				CoverUrl = GetValueOrDefault(cover, "source") as string
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

	public class GetEventsConfiguration
	{
		public GetEventsConfiguration(FacebookEventsOptions eventsConfiguration, string identifier)
		{
			EventsConfiguration = eventsConfiguration;
			Identifier = identifier;
		}

		public FacebookEventsOptions EventsConfiguration { get; }
		public string Identifier { get; }
	}

	public class GetEventsResponse
	{
		public GetEventsResponse(IEnumerable<FacebookEvent> events)
		{
			Events = events;
		}

		public IEnumerable<FacebookEvent> Events { get; }
	}
}
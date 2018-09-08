﻿using System.Collections.Generic;
using LiveLink.Services.Models;
using Skybrud.Social.Umbraco.Facebook.PropertyEditors.OAuth;
using Umbraco.Core.Models;

namespace LiveLink.Services.FacebookEventsService
{
	public interface IFacebookEventsService
	{
		IEnumerable<LiveLinkEvent> GetEventsForVenues(int? limit = 10);
		IEnumerable<LiveLinkEvent> GetEventsForVenue(IPublishedContent venueContent, int? limit);
	}
}
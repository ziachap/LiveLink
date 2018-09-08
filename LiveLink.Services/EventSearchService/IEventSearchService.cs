using System.Collections.Generic;
using Umbraco.Core.Models;

namespace LiveLink.Services.EventSearchService
{
	public interface IEventSearchService
	{
		IEnumerable<IPublishedContent> GetVenueEvents(GetEventsConfiguration configuration);
	}
}
using System.Collections.Generic;
using LiveLink.Services.Models.ViewModels;
using Umbraco.Core.Models;

namespace LiveLink.Services.EventSearchService
{
	public interface IEventSearchService
	{
		IEnumerable<IPublishedContent> GetVenueEvents(GetEventsConfiguration configuration);
	}
}
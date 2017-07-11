using System.Collections.Generic;
using LiveLink.Services.Processors;

namespace LiveLink.Services.Models.ViewModels
{
	public class BrowseViewModel
	{
		[VenueEvents]
		public IEnumerable<VenueViewModel> Venues { get; set; }
	}
}

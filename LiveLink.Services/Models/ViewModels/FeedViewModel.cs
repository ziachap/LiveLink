using System.Collections.Generic;
using System.Globalization;
using LiveLink.Services.Processors;
using Our.Umbraco.Ditto;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace LiveLink.Services.Models.ViewModels
{
	public class FeedViewModel : RenderModel
	{
		public FeedViewModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}

		public FeedViewModel(IPublishedContent content) : base(content)
		{
		}

		[UmbracoProperty("contentTitle")]
		public string Title { get; set; }

		public string UppercaseTitle => Title?.ToUpper();

		[UmbracoProperty("contentDescription")]
		public string Description { get; set; }

		[DittoIgnore]
		public IEnumerable<EventViewModel> Events { get; set; }

		[DittoIgnore]
		public IEnumerable<LocationOption> Countries { get; set; }

		[DittoIgnore]
		public IEnumerable<LocationOption> Cities { get; set; }

		[DittoIgnore]
		public IEnumerable<LocationOption> Venues { get; set; }
	}
}
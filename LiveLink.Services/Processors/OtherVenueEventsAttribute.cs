using System;
using System.Linq;
using Gibe.DittoServices.ModelConverters;
using LiveLink.Services.Models;
using LiveLink.Services.Models.ViewModels;

namespace LiveLink.Services.Processors
{
	public class OtherVenueEventsAttribute : InjectableProcessorAttribute
	{
		private readonly int _limit;

		public Func<IModelConverter> ModelConverter => Inject<IModelConverter>();

		public OtherVenueEventsAttribute(int limit = default(int))
		{
			_limit = limit;
		}

		public override object ProcessValue()
		{
			var venue = Context.Content.Parent;

			var events = venue.Children.Where(x => x.Id != Context.Content.Id);

			var limitedEvents = _limit != default(int) ? events.Take(_limit) : events;

			return ModelConverter().ToModel<OtherEventModel>(limitedEvents).ToList();
		}
	}
}

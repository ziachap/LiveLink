using System;
using LiveLink.Services.EventSearchService;

namespace LiveLink.Services.Processors
{
    public class VenueEventsAttribute : InjectableProcessorAttribute
    {
        public Func<IEventSearchService> EventSearchService => Inject<IEventSearchService>();

        public override object ProcessValue()
        {
            return EventSearchService().GetVenueEvents(new GetEventsConfiguration
            {
                BoundMaxX = 360,
                BoundMinX = -360,
                BoundMaxY = 360,
                BoundMinY = -360
            });
        }
    }
}
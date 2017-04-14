using System;
using Skybrud.Social.Json;

namespace LiveLink.Services.FacebookEventsService
{
    public class FacebookEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string TicketUri { get; set; }
		public string CoverUrl { get; set; }

		public override string ToString() => Name;
    }
}
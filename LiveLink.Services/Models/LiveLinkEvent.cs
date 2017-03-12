using System;

namespace LiveLink.Services.Models
{
    public class LiveLinkEvent
    {
        public string FacebookEventIdentifier { get; set; }
        public int VenueNodeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string TicketUri { get; set; }

        public override string ToString() => Title;
    }
}
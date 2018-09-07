using System.Collections.Generic;
using LiveLink.Services.Models;

namespace LiveLink.Services.EventImportService
{
    public interface IEventImportService
    {
        void SaveEvents(IEnumerable<LiveLinkEvent> events);
    }
}
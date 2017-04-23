using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gibe.DittoServices;
using LiveLink.Services.Models;

namespace LiveLink.Services.EventImportService
{
    public interface IEventImportService
    {
        void SaveEvents(IEnumerable<LiveLinkEvent> events);
    }
}

using System.Collections.Generic;

namespace LiveLink.Services.TagService
{
	public interface ISmartTagService
	{
		IEnumerable<string> ExtractTags(string text);
	}
}

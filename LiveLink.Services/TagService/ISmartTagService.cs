using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LiveLink.Services.TagService
{
	public interface ISmartTagService
	{
		IEnumerable<string> ExtractTags(string text);
	}
}

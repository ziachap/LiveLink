using System.Collections.Generic;
using System.Linq;
using Gibe.DittoProcessors.Media.Models;

namespace LiveLink.Services.ContentSearchService.Models
{
	public class ContentSearchResults
	{
		public ContentSearchResults(IEnumerable<IContentSearchResult> results, ContentSearchConfiguration configuration)
		{
			Results = results;
			Configuration = configuration;
			Count = results.Count();
		}

		public IEnumerable<IContentSearchResult> Results { get; }

		public int Count { get; }

		public ContentSearchConfiguration Configuration { get; }
	}

	public interface IContentSearchResult
	{
		string Title { get; }

		string Subtitle { get; }

		string Description { get; }

		string Link { get; }

		MediaImageModel Image { get; }

		bool IsEvent { get; }

		bool IsVenue { get; }
	}
}
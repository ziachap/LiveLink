using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;

namespace LiveLink.Services.TagService
{
	public class SmartTagService : ISmartTagService
	{
		// TODO: This really needs to go in a config or db
		private readonly IEnumerable<TagCriteria> _tags = new[]
		{
			new TagCriteria("Drum & Bass", new[]
			{
				"dnb",
				"drum n bass",
				"drum and bass"
			}),
			new TagCriteria("Dubstep", Enumerable.Empty<string>()),
			new TagCriteria("House", Enumerable.Empty<string>()),
			new TagCriteria("Garage", new[]
			{
				"2 step",
				"2step"
			}),
			new TagCriteria("Grime", Enumerable.Empty<string>()),
			new TagCriteria("R&B", new[]
			{
				"rnb"
			}),
			new TagCriteria("Techno", Enumerable.Empty<string>()),
			new TagCriteria("Jungle", Enumerable.Empty<string>()),
			new TagCriteria("Disco", Enumerable.Empty<string>())
		};

		public IEnumerable<string> ExtractTags(string text)
		{
			return _tags
				.Where(x => ContainsTag(text, x.Tag) || x.Aliases.Any(alias => ContainsTag(text, alias)))
				.Select(x => x.Tag)
				.ToList();
		}

		private bool ContainsTag(string text, string tag) => text.InvariantContains(tag);

		private class TagCriteria
		{
			public TagCriteria(string tag, IEnumerable<string> aliases)
			{
				Tag = tag;
				Aliases = aliases;
			}

			public string Tag { get; }
			public IEnumerable<string> Aliases { get; }
		}
	}
}
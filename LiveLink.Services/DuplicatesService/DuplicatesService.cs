﻿using System.Linq;
using LiveLink.Services.EventSearchService;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace LiveLink.Services.DuplicatesService
{
	public class DuplicatesService : IDuplicatesService
	{
		private readonly IContentService _contentService;
		private readonly IEventSearchService _eventSearchService;
		private readonly ITextComparisonService _textComparisonService;

		private const double TextSimilarityThreshold = 0.95;

		public DuplicatesService(IContentService contentService, IEventSearchService eventSearchService,
			ITextComparisonService textComparisonService)
		{
			_contentService = contentService;
			_eventSearchService = eventSearchService;
			_textComparisonService = textComparisonService;
		}

		public void RemoveDuplicates()
		{
			var allEvents = _eventSearchService.GetVenueEvents(new GetEventsConfiguration()).ToList();
			
			for (var i = 0; i < allEvents.Count; i++)
			{
				for (var j = i + 1; i < allEvents.Count; i++)
				{
					if (AreEqual(allEvents[i], allEvents[j]))
					{
						var oldestContent = Oldest(allEvents[i], allEvents[j]);
						Delete(oldestContent);
					}
				}
			}
		}

		private void Delete(IPublishedContent publishedContent)
		{
			var content = _contentService.GetById(publishedContent.Id);
			if (content != null)
			{
				_contentService.Delete(content);
			}
		} 

		private IPublishedContent Oldest(IPublishedContent a, IPublishedContent b) => a.UpdateDate < b.UpdateDate ? a : b;

		private bool AreEqual(IPublishedContent a, IPublishedContent b)
			=> _textComparisonService.PercentageSimilarity(ContentTitle(a), ContentTitle(b)) > TextSimilarityThreshold;

		private string ContentTitle(IPublishedContent content)
			=> content.GetPropertyValue<string>("contentTitle");
	}
}
using System.Collections.Generic;
using System.Linq;
using Gibe.DittoServices.ModelConverters;
using Gibe.UmbracoWrappers;
using LiveLink.Services.ContentSearchService.Models;
using LiveLink.Services.ExamineService;
using Umbraco.Core.Models;

namespace LiveLink.Services.ContentSearchService
{
    public class ContentSearchService : IContentSearchService
    {
        private readonly IExamineSearchProviderWrapper _examineSearchProviderWrapper;
        private readonly IModelConverter _modelConverter;
        private readonly IUmbracoWrapper _umbracoWrapper;

        public ContentSearchService(IUmbracoWrapper umbracoWrapper,
            IExamineSearchProviderWrapper examineSearchProviderWrapper,
            IModelConverter modelConverter)
        {
            _umbracoWrapper = umbracoWrapper;
            _examineSearchProviderWrapper = examineSearchProviderWrapper;
            _modelConverter = modelConverter;
        }

        public ContentSearchResults Search(ContentSearchConfiguration config)
        {
            var provider = _examineSearchProviderWrapper.ContentSearcher();
            var results = _umbracoWrapper.TypedSearch(config.Text, true, provider.Name).Take(config.Limit);
            var contentResults = AsContentSearchResults(results);

            return new ContentSearchResults(contentResults, config);
        }

        private IEnumerable<IContentSearchResult> AsContentSearchResults(IEnumerable<IPublishedContent> results)
        {
            foreach (var content in results)
                switch (content.DocumentTypeAlias)
                {
                    case "event":
                        yield return _modelConverter.ToModel<EventContentSearchResult>(content);
                        break;
                    case "venue":
                        yield return _modelConverter.ToModel<VenueContentSearchResult>(content);
                        break;
                }
        }
    }
}
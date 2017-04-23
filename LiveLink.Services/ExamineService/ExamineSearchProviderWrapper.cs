using Examine;
using Examine.Providers;

namespace LiveLink.Services.ExamineService
{
	public class ExamineSearchProviderWrapper : IExamineSearchProviderWrapper
	{
		public BaseSearchProvider EventSearcher() 
			=> ExamineManager.Instance.SearchProviderCollection["EventSearcher"];
	}
}
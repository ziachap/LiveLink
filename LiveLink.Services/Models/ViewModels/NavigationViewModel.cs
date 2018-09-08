using LiveLink.Services.Processors;
using Our.Umbraco.Ditto;

namespace LiveLink.Services.Models.ViewModels
{
	[DittoCache(CacheBy = DittoCacheBy.TargetType, CacheDuration = 60)]
	public class NavigationViewModel
	{
		[Navigation] public NavigationNode Navigation { get; set; }

		[DittoIgnore] public LoginForm LoginForm { get; set; }
	}
}
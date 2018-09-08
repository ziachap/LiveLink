using System.Linq;
using System.Web.Mvc;
using Gibe.DittoProcessors.Processors;
using Gibe.DittoServices.ModelConverters;
using Gibe.UmbracoWrappers;
using LiveLink.Services.Models;
using Umbraco.Web;

namespace LiveLink.Services.Processors
{
	public class CountriesAttribute : TestableDittoProcessorAttribute
	{
		private readonly IModelConverter _modelConverter;
		private readonly IUmbracoWrapper _umbracoWrapper;

		public CountriesAttribute()
		{
			_umbracoWrapper = DependencyResolver.Current.GetService<IUmbracoWrapper>();
			_modelConverter = DependencyResolver.Current.GetService<IModelConverter>();
		}

		public CountriesAttribute(IUmbracoWrapper umbracoWrapper, IModelConverter modelConverter)
		{
			_umbracoWrapper = umbracoWrapper;
			_modelConverter = modelConverter;
		}

		public override object ProcessValue()
		{
			return _umbracoWrapper.AncestorOrSelf(Context.Content, 1)
				.Children.First(x => x.DocumentTypeAlias.Equals("locations"))
				.Children()
				.Where(x => x.Children.Any())
				.Select(x => _modelConverter.ToModel<LocationOption>(x))
				.ToList();
		}
	}
}
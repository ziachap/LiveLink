using System.Linq;
using System.Web.Mvc;
using Gibe.DittoServices.ModelConverters;
using Gibe.UmbracoWrappers;
using LiveLink.Services.Models;
using Our.Umbraco.Ditto;
using Umbraco.Core.Models;

namespace LiveLink.Services.Processors
{
	public class NavigationAttribute : DittoProcessorAttribute
	{
		private readonly IModelConverter _modelConverter;
		private readonly IUmbracoWrapper _umbracoWrapper;

		public NavigationAttribute(IUmbracoWrapper umbracoWrapper, IModelConverter modelConverter)
		{
			_umbracoWrapper = umbracoWrapper;
			_modelConverter = modelConverter;
		}

		public NavigationAttribute()
		{
			_umbracoWrapper = DependencyResolver.Current.GetService<IUmbracoWrapper>();
			_modelConverter = DependencyResolver.Current.GetService<IModelConverter>();
		}

		public override object ProcessValue()
		{
			return NavigationNode(_umbracoWrapper.AncestorOrSelf(Context.Content, 1));
		}

		private NavigationNode NavigationNode(IPublishedContent content)
		{
			var node = _modelConverter.ToModel<NavigationNode>(content);
			node.Active = Context.Content.Url == node.Url;
			node.Children = content.Children.Select(NavigationNode);
			return node;
		}
	}
}
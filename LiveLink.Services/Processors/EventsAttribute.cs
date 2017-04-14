using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gibe.DittoProcessors.Processors;
using Gibe.DittoServices.ModelConverters;
using Gibe.UmbracoWrappers;
using LiveLink.Services.Models.ViewModels;

namespace LiveLink.Services.Processors
{
	public class EventsAttribute : TestableDittoProcessorAttribute
	{
		private readonly IUmbracoWrapper _umbracoWrapper;
		private readonly IModelConverter _modelConverter;

		public EventsAttribute()
		{
			_umbracoWrapper = DependencyResolver.Current.GetService<IUmbracoWrapper>();
			_modelConverter = DependencyResolver.Current.GetService<IModelConverter>();
		}

		public EventsAttribute(IUmbracoWrapper umbracoWrapper, IModelConverter modelConverter)
		{
			_umbracoWrapper = umbracoWrapper;
			_modelConverter = modelConverter;
		}

		public override object ProcessValue()
		{
			return _umbracoWrapper.AncestorOrSelf(Context.Content, 1)
				.Children.First(x => x.DocumentTypeAlias.Equals("venues"))
				.Children.SelectMany(x => x.Children)
				.Select(x => _modelConverter.ToModel<EventViewModel>(x));
		}
	}
}

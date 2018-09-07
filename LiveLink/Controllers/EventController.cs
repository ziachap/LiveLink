using System.Globalization;
using System.Web.Mvc;
using Gibe.DittoServices.ModelConverters;
using Gibe.UmbracoWrappers;
using LiveLink.Services.Models.ViewModels;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace LiveLink.Controllers
{
	public class EventController : RenderMvcController
	{
		private readonly IModelConverter _modelConverter;
		private readonly IUmbracoWrapper _umbracoWrapper;

		public EventController(IModelConverter modelConverter, IUmbracoWrapper umbracoWrapper)
		{
			_modelConverter = modelConverter;
			_umbracoWrapper = umbracoWrapper;
		}

		public override ActionResult Index(RenderModel model)
		{
            // Currently using DittoViews so this is disabled for now
			//var viewModel = _modelConverter.ToModel<EventViewModel>(model.Content);

			return base.Index(model);
		}

		public ActionResult AjaxIndex(int id)
		{
			var node = _umbracoWrapper.TypedContent(id);

			return Index(new RenderModel(node, CultureInfo.CurrentCulture));
		}
	}
}

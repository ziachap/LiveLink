using System.Web.Mvc;
using System.Web.Routing;
using Gibe.UmbracoWrappers;
using Umbraco.Web.Mvc;

namespace LiveLink.Areas.API.Controllers
{
    public class AjaxEventController : Controller
    {
	    private readonly IUmbracoWrapper _umbracoWrapper;

	    public AjaxEventController(IUmbracoWrapper umbracoWrapper)
	    {
		    _umbracoWrapper = umbracoWrapper;
	    }

	    public ActionResult Index(int id)
	    {
			// TODO: Error catching/logging

		    var node = _umbracoWrapper.TypedContent(id);

			return new RedirectToUmbracoPageResult(node);
        }
    }
}
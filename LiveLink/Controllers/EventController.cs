using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace LiveLink.Controllers
{
	public class EventController : RenderMvcController
	{
	    public override ActionResult Index(RenderModel model)
	    {
	        

            //Do some stuff here, then return the base method
            return base.Index(model);
        }

    }
}

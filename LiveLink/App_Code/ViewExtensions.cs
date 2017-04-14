using System.IO;
using System.Web;
using System.Web.Mvc;

namespace LiveLink.App_Code
{
	public static class ViewExtensions
	{
		public static IHtmlString RenderRawContent(this HtmlHelper helper, string serverPath)
		{
			string filePath = HttpContext.Current.Server.MapPath(serverPath);

			//load from file
			StreamReader streamReader = File.OpenText(filePath);
			string markup = streamReader.ReadToEnd();
			streamReader.Close();

			return new HtmlString(markup);

		}
	}
}
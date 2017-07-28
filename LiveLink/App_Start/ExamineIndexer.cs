using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Examine.LuceneEngine;
using Gibe.UmbracoWrappers;
using LiveLink.Services.NumericIndexFormatter;
using Lucene.Net.Documents;
using umbraco.NodeFactory;

namespace LiveLink.App_Start
{
	public class ExamineIndexer
	{
		public static void EventIndexer_DocumentWriting(object sender, DocumentWritingEventArgs e)
		{
			var document = e.Document;

			var id = int.Parse(e.Fields["id"]);

			var formatter = new NumericIndexFormatter();
			var parent = new Node(id).Parent;

			var contentLongitude = parent.GetProperty("contentLongitude").Value;
			if (!string.IsNullOrEmpty(contentLongitude))
			{
				var contentLongitudeFormatted = formatter.Format(double.Parse(contentLongitude));
					document.Add(new Field("contentLongitude", contentLongitudeFormatted,
				Field.Store.YES, Field.Index.NOT_ANALYZED));
			}

			var contentLatitude = parent.GetProperty("contentLatitude").Value;
			if (!string.IsNullOrEmpty(contentLongitude))
			{
				var contentLatitudeFormatted = formatter.Format(double.Parse(contentLatitude));
				document.Add(new Field("contentLatitude", contentLatitudeFormatted,
			Field.Store.YES, Field.Index.NOT_ANALYZED));
			}
		}
	}
}
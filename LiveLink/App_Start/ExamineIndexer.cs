using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Examine.LuceneEngine;
using Gibe.UmbracoWrappers;
using LiveLink.Services.IndexFormatters;
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

			var doubleFormatter = new DoubleIndexFormatter();
			var intFormatter = new IntegerIndexFormatter();

			var venue = new Node(id).Parent;
			var contentLongitude = venue.GetProperty("contentLongitude").Value;
			if (!string.IsNullOrEmpty(contentLongitude))
			{
				var contentLongitudeFormatted = doubleFormatter.Format(double.Parse(contentLongitude));
					document.Add(new Field("contentLongitude", contentLongitudeFormatted,
				Field.Store.YES, Field.Index.NOT_ANALYZED));
			}

			var contentLatitude = venue.GetProperty("contentLatitude").Value;
			if (!string.IsNullOrEmpty(contentLongitude))
			{
				var contentLatitudeFormatted = doubleFormatter.Format(double.Parse(contentLatitude));
				document.Add(new Field("contentLatitude", contentLatitudeFormatted,
					Field.Store.YES, Field.Index.NOT_ANALYZED));
			}

			var city = venue.Parent;
			document.Add(new Field("city", intFormatter.Format(city.Id),
				Field.Store.YES, Field.Index.NOT_ANALYZED));

			var country = city.Parent;
			document.Add(new Field("country", intFormatter.Format(country.Id),
				Field.Store.YES, Field.Index.NOT_ANALYZED));
		}
	}
}
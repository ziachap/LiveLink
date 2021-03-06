﻿using System.Drawing;
using System.Web;
using Gibe.DittoProcessors.Media.Models;
using Gibe.DittoProcessors.Processors;

namespace LiveLink.Services.Processors
{
	// TODO: Put default image in web.config
	public class ImagePickerOrDefaultImage : ImagePickerAttribute
	{
		public override object ProcessValue()
		{
			var image = base.ProcessValue();

			if (image == null)
			{
				// TODO: Inject http context
				// TODO: Cache default image width/height
				var defaultImagePath = HttpContext.Current.Server.MapPath("/images/bg.png");
				var defaultImage = new Bitmap(Image.FromFile(defaultImagePath));
				return new MediaImageModel
				{
					Url = "/images/bg.jpg",
					Height = defaultImage.Height,
					Width = defaultImage.Width
				};
			}

			return image;
		}
	}
}
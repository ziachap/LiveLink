using System;
using System.IO;
using System.Linq;
using System.Net;
using log4net;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace LiveLink.Services.EventImportService
{
	public class UmbracoImageRetriever : IUmbracoImageRetriever
	{
		private readonly IMediaService _mediaService;
		private readonly ILog _log;

		public UmbracoImageRetriever(IMediaService mediaService, ILog log)
		{
			_mediaService = mediaService;
			_log = log;
		}

		public int? RetrieveAndSaveImage(string url, string filename)
		{
			// TODO: Gallery images
			// TODO: Clean this whole thing up

			if (string.IsNullOrEmpty(url)) return null;

			try
			{
				var fileName = filename;
				var folderExists = false;
				var folderId = 1153; //Folder Id in Media Library TODO: This is dumb

				var request = WebRequest.Create(url);
				request.Timeout = 30000;

				using (var response = (HttpWebResponse)request.GetResponse())
				using (var stream = response.GetResponseStream())
				{
					if (response.StatusCode == HttpStatusCode.OK)
					{
						Stream streamCopy = new MemoryStream();

						stream.CopyTo(streamCopy);

						var assetID = _mediaService.GetChildren(folderId)
							.Where(c => c.Name == fileName)
							.Select(c => c.Id)
							.FirstOrDefault();

						var uri = new Uri(url);

						var origFilename = Path.GetFileName(uri.LocalPath);

						if (assetID != default(int))
						{
							try
							{
								var existingFile = _mediaService.GetById(assetID);
								existingFile.SetValue("umbracoFile", origFilename, streamCopy);
								_mediaService.Save(existingFile);
								return existingFile.Id;
							}
							catch (Exception ex)
							{
								throw new Exception("There was a problem updating Image - " + assetID, ex);
							}
						}

						try
						{
							var mediaFile = _mediaService.CreateMedia(fileName, folderId, "Image");
							mediaFile.SetValue("umbracoFile", origFilename, streamCopy);
							_mediaService.Save(mediaFile);
							return mediaFile.Id;
						}
						catch (Exception ex)
						{
							throw new Exception("There was a problem saving the image - " + fileName, ex);
						}

					}
				}
			}
			catch (Exception ex)
			{
				_log.Error("Image retrieval failed", ex);
			}
			
			return null;
		}
	}
}
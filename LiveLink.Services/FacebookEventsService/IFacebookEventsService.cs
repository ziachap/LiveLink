using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Gibe.UmbracoWrappers;
using LiveLink.Services.Models;
using Skybrud.Social.Facebook.Options.Events;
using Skybrud.Social.Umbraco.Facebook.PropertyEditors.OAuth;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace LiveLink.Services.FacebookEventsService
{
    public interface IFacebookEventsService
    {
        IEnumerable<LiveLinkEvent> GetEventsForVenues(int? limit = 10);
    }

    public class FacebookEventsService : IFacebookEventsService
    {
        private readonly IFacebookApiWrapper _facebookApiWrapper;
        private readonly IUmbracoWrapper _umbracoWrapper;
        

        public FacebookEventsService(IFacebookApiWrapper facebookApiWrapper, IUmbracoWrapper umbracoWrapper)
        {
            _facebookApiWrapper = facebookApiWrapper;
            _umbracoWrapper = umbracoWrapper;
        }

        public IEnumerable<LiveLinkEvent> GetEventsForVenues(int? limit = 10)
        {
            var authData = _umbracoWrapper.GetPropertyValue<FacebookOAuthData>(Settings(),
                "settingsFacebookOAuth");

            var eventsOptions = new FacebookEventsOptions
            {
                Limit = limit
			};

            return Venues().SelectMany(x => GetEventsForVenue(x, authData, eventsOptions)).ToList();
        }

        public IEnumerable<LiveLinkEvent> GetEventsForVenue(IPublishedContent venue,
            FacebookOAuthData authData, FacebookEventsOptions options)
        {
            var pageId = _umbracoWrapper.GetPropertyValue<string>(venue,
                "developerFacebookPageIdentifier");

            var events = _facebookApiWrapper.GetEvents(authData, options, pageId);

	        foreach (var e in events.ToList())
	        {
		        
	        }

            return events.Select(x => ToLiveLinkEvent(x, venue.Id));
        }

	    private int? RetrieveAndSaveImage(string url, string filename)
	    {
			// TODO: Gallery images
			// TODO: Clean this shit up

		    if (string.IsNullOrEmpty(url)) return null;

			IMediaService ms = ApplicationContext.Current.Services.MediaService;

			string fileName = filename;
			bool folderExists = false;
			int folderId = 1153; //Folder Id in Media Library

			var request = WebRequest.Create(url);
			request.Timeout = 30000;

			using (var response = (HttpWebResponse)request.GetResponse())
			using (Stream stream = response.GetResponseStream())
			{
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					Stream streamCopy = new MemoryStream();

					stream.CopyTo(streamCopy);

					IEnumerable<IMedia> getAllChildItems = ms.GetChildren(folderId);
					#region "Either Add the File or Update the Existing one"
					int assetID = 0;
					assetID = ms.GetChildren(folderId).Where(c => c.Name == fileName).Select(c => c.Id).FirstOrDefault();

					string origFilename = "";

					Uri uri = new Uri(url);

					origFilename = System.IO.Path.GetFileName(uri.LocalPath);


					if (assetID > 0)
					{
						try
						{
							IMedia existingFile = ms.GetById(assetID);
							existingFile.SetValue("umbracoFile", origFilename, streamCopy);
							ms.Save(existingFile);
							return existingFile.Id;
						}
						catch
						{
							throw new Exception("There was a problem updating Image - " + assetID);
						}

					}
					else
					{
						try
						{
							IMedia mediaFile = ms.CreateMedia(fileName, folderId, "Image");
							mediaFile.SetValue("umbracoFile", origFilename, streamCopy);
							ms.Save(mediaFile);
							return mediaFile.Id;

						}
						catch (Exception ex)
						{

							throw new Exception("There was a problem saving the image - " + fileName);
						}

					}
					#endregion

				}

			}

		    return null;
	    }

	    private LiveLinkEvent ToLiveLinkEvent(FacebookEvent facebookEvent, int venueNodeId)
        {
            return new LiveLinkEvent
            {
                Title = facebookEvent.Name,
                Description = facebookEvent.Description,
                StartDateTime = facebookEvent.StartDateTime,
                EndDateTime = facebookEvent.EndDateTime,
                VenueNodeId = venueNodeId,
                FacebookEventIdentifier = facebookEvent.Id,
                TicketUri = facebookEvent.TicketUri,
				Thumbnail = RetrieveAndSaveImage(facebookEvent.CoverUrl, facebookEvent.Id)
			};
        }

        private IPublishedContent Settings()
            => _umbracoWrapper.TypedContentAtRoot().First(x => x.DocumentTypeAlias.Equals("settings"));

        private IEnumerable<IPublishedContent> Venues()
            => Settings().Children.First(x => x.DocumentTypeAlias.Equals("venues")).Children;
    }
}
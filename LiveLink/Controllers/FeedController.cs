using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Gibe.DittoServices.ModelConverters;
using Gibe.UmbracoWrappers;
using LiveLink.Areas.API.Controllers;
using LiveLink.Services.EventSearchService;
using LiveLink.Services.Extensions;
using LiveLink.Services.Models;
using LiveLink.Services.Models.ViewModels;
using Umbraco.Core.Models;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using FeedViewModel = LiveLink.Services.Models.ViewModels.FeedViewModel;

namespace LiveLink.Controllers
{
	public class FeedController : RenderMvcController
	{
		private readonly IModelConverter _modelConverter;
		private readonly IUmbracoWrapper _umbracoWrapper;
		private readonly IEventSearchService _eventSearchService;

		public FeedController(IModelConverter modelConverter,
			IUmbracoWrapper umbracoWrapper, 
			IEventSearchService eventSearchService)
		{
			_modelConverter = modelConverter;
			_umbracoWrapper = umbracoWrapper;
			_eventSearchService = eventSearchService;
		}

		public ActionResult Index(RenderModel model, GetEventsConfiguration configuration)
		{
			OverridePaging(configuration);
			ValidateLocationSelection(configuration);

			var viewModel = _modelConverter.ToModel<FeedViewModel>(model.Content);
			
			viewModel.Countries = Countries(model.Content, configuration);

			if (!configuration.CountryId.HasValue)
			{
				configuration.CountryId = viewModel.Countries.First().Id;
			}

			viewModel.Cities = SubLocations(configuration.CountryId, configuration.CityId);
			viewModel.Venues = SubLocations(configuration.CityId, configuration.VenueId);
			viewModel.Events = _eventSearchService.GetVenueEvents(configuration).ToModel<EventViewModel>();

			return View("~/Views/Feed.cshtml", viewModel);
		}

		private void OverridePaging(GetEventsConfiguration configuration)
		{
			configuration.Page = 1;
			configuration.ItemsPerPage = 12;
		}

		private void ValidateLocationSelection(GetEventsConfiguration configuration)
		{
			if (!configuration.CountryId.HasValue)
			{
				configuration.CityId = null;
				configuration.VenueId = null;
				return;
			}

			var validCities = _umbracoWrapper.TypedContent(configuration.CountryId.Value)
				.Children;
			if (!configuration.CityId.HasValue || validCities.All(x => x.Id != configuration.CityId))
			{
				configuration.CityId = null;
				configuration.VenueId = null;
				return;
			}

			var validVenues = _umbracoWrapper.TypedContent(configuration.CityId.Value)
				.Children;
			if (!configuration.VenueId.HasValue ||  validVenues.All(x => x.Id != configuration.VenueId.Value))
			{
				configuration.VenueId = null;
			}
		}

		public IEnumerable<LocationOption> Countries(IPublishedContent content, GetEventsConfiguration configuration)
		{
			return _umbracoWrapper.AncestorOrSelf(content, 1)
				.Children
				.First(x => x.DocumentTypeAlias.Equals("locations"))
				.Children
				.Where(x => x.Children.Any())
				.Select(x => Option(x, configuration.CountryId))
				.ToList();
		}

		private IEnumerable<LocationOption> SubLocations(int? parentId, int? selectedId)
		{
			if (!parentId.HasValue)
			{
				return Enumerable.Empty<LocationOption>();
			}
			return _umbracoWrapper.TypedContent(parentId.Value)
				.Children
				.Where(x => x.Children.Any())
				.Select(x => Option(x, selectedId))
				.ToList();
		}
		
		private LocationOption Option(IPublishedContent content, int? selectedId)
		{
			var option = _modelConverter.ToModel<LocationOption>(content);
			option.Selected = option.Id == selectedId;
			option.Disabled = false;
			return option;
		}
	}
}
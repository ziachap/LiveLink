using LiveLink.Services.FacebookEventsService;
using Gibe.UmbracoWrappers;
using Gibe.DittoServices.ModelConverters;
using LiveLink.Services.EventImportService;
using LiveLink.Services.ExamineService;
using LiveLink.Services.EventSearchService;
using Umbraco.Core.Services;
using Umbraco.Core;
using LiveLink.Services.AuthenticationService;
using LiveLink.Services.ContentSearchService;
using LiveLink.Services.DuplicatesService;
using LiveLink.Services.IndexFormatters;
using LiveLink.Services.TagService;
using IMediaService = Gibe.DittoProcessors.Media.IMediaService;
using MediaService = Gibe.DittoProcessors.Media.MediaService;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(LiveLink.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(LiveLink.App_Start.NinjectWebCommon), "Stop")]

namespace LiveLink.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
				kernel.Bind<HttpContext>().ToMethod(c => HttpContext.Current);

				BindUmbracoServices(kernel);

				kernel.Bind<IUmbracoWrapper>().To<DefaultUmbracoWrapper>();
				kernel.Bind<IMediaService>().To<MediaService>();
				kernel.Bind<IModelConverter>().To<DittoModelConverter>();

				kernel.Bind<IEventImportService>().To<EventImportService>();
				kernel.Bind<ISmartTagService>().To<SmartTagService>();
                kernel.Bind<IFacebookApiWrapper>().To<FacebookApiWrapper>();
                kernel.Bind<IFacebookEventsService>().To<FacebookEventsService>();
                kernel.Bind<IContentSearchService>().To<ContentSearchService>();

				kernel.Bind<IEventSearchService>().To<EventSearchService>();
				kernel.Bind<IExamineService>().To<ExamineService>();
				kernel.Bind<IExamineSearchProviderWrapper>().To<ExamineSearchProviderWrapper>();
				kernel.Bind<IAuthenticationService>().To<AuthenticationService>();
				kernel.Bind<IDuplicatesService>().To<DuplicatesService>();
				kernel.Bind<ITextComparisonService>().To<TextComparisonService>();

				kernel.Bind<IIndexFormatter<double>>().To<DoubleIndexFormatter>();
				kernel.Bind<IIndexFormatter<int>>().To<IntegerIndexFormatter>();
				kernel.Bind<IIndexFormatter<DateTime>>().To<DatetimeIndexFormatter>();


				RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

	    private static void BindUmbracoServices(StandardKernel kernel)
	    {
			kernel.Bind<Umbraco.Web.Security.MembershipHelper>().ToMethod(x => new Umbraco.Web.Security.MembershipHelper(Umbraco.Web.UmbracoContext.Current));
			kernel.Bind<IMemberService>().ToMethod(x => ApplicationContext.Current.Services.MemberService);
			kernel.Bind<IContentService>().ToMethod(x => ApplicationContext.Current.Services.ContentService);
			kernel.Bind<Umbraco.Core.Services.IMediaService>().ToMethod(x => ApplicationContext.Current.Services.MediaService);
		}

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }        
    }
}

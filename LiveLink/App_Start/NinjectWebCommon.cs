using LiveLink.Services.FacebookEventsService;
using Gibe.UmbracoWrappers;
using Gibe.DittoServices.ModelConverters;
using LiveLink.Services.EventImportService;
using LiveLink.Services.ExamineService;
using LiveLink.Services.EventSearchService;

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

                kernel.Bind<IUmbracoWrapper>().To<DefaultUmbracoWrapper>();
				kernel.Bind<IModelConverter>().To<DittoModelConverter>();

				kernel.Bind<IEventImportService>().To<EventImportService>();
                kernel.Bind<IFacebookApiWrapper>().To<FacebookApiWrapper>();
                kernel.Bind<IFacebookEventsService>().To<FacebookEventsService>();

				kernel.Bind<IEventSearchService>().To<EventSearchService>();
				kernel.Bind<IExamineService>().To<ExamineService>();
				kernel.Bind<IExamineSearchProviderWrapper>().To<ExamineSearchProviderWrapper>();


				RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
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

using System;
using Umbraco.Core;
using System.Web.Http;
using System.Web.Optimization;
using Umbraco.Web.Mvc;
using umbraco.BusinessLogic;
using DevTrends.MvcDonutCaching;
using .Controllers;
using Umbraco.Core.Services;


namespace .App_Start
{
     public class ApplicationStartup : IApplicationEventHandler
    {
        public ApplicationStartup()
        {
            ContentService.Published += PublishedEventHandler;
            ContentService.UnPublished += UnpublishedEventHandler;
            ContentService.Moved += MovedEventHandler;
            ContentService.Trashed += TrashedEventHandler;
            MediaService.Saved += MediaSavedEventHandler;
            MediaService.Deleted += MediaDeletedEventHandler;
        }

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }


        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
			DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(DefaultController));
        }



         private void TrashedEventHandler(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            ClearCache();
        }

        private void MovedEventHandler(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            ClearCache();
        }

        private void MediaDeletedEventHandler(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            ClearCache();
        }

        private void MediaSavedEventHandler(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            ClearCache();
        }

        private void UnpublishedEventHandler(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            ClearCache();
        }

        private void PublishedEventHandler(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                //Clear all output cache so everything is refreshed.
                var cacheManager = new OutputCacheManager();
                cacheManager.RemoveItems();
            }
            catch (Exception ex)
            {
                Log.Add(LogTypes.Error, -1, string.Format("Exception: {0} - StackTrace: {1}", ex.Message, ex.StackTrace));
            }
        }


        

    }
}

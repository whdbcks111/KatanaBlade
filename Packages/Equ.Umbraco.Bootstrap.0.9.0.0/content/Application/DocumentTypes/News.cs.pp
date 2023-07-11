using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Services;
using UmbracoAOP.EventSubscriber;
using Vega.USiteBuilder;

namespace $rootnamespace$.Application.DocumentTypes
{
    [DocumentType(AllowedTemplates = new string[] { "ContentPage" }, AllowedChildNodeTypes = new Type[] { typeof(ContentPage) }, IconUrl = "icon-umb-content", AllowAtRoot=false)]
    public class News : DocumentBase
    {
        [DocumentTypeProperty(UmbracoPropertyType.RichtextEditor, Tab = DocumentBase.CmsTabs.Details, Name = "Content")]
        public String Content { get; set; }

        [DocumentTypeProperty(UmbracoPropertyType.DatePickerWithTime, Tab = DocumentBase.CmsTabs.Details, Name = "News Date")]
        public DateTime NewsDate { get; set; }

        public static IOrderedEnumerable<News> GetNewsList()
        {
            var newsOverviewNode = ContentHelper.GetByNode(umbraco.uQuery.GetNodesByType("NewsOverview").FirstOrDefault());
            return newsOverviewNode.GetChildren<News>().OrderBy(x=>x.NewsDate);
        }
		
		[UmbracoEvent("News")]
        public static void NewsEvent_OnSaved(IContentService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            try
            {
                foreach (var item in e.SavedEntities)
                {
                    //if the user doesnt enter a date for PublishDate then we will populate it with the current when first published. 
                    if (item.Properties["newsDate"].Value == null || string.IsNullOrEmpty(item.Properties["newsDate"].Value.ToString()))
                    {
                        item.Properties["newsDate"].Value = DateTime.Now.ToString();
                        sender.SaveAndPublishWithStatus(item);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
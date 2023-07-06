using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vega.USiteBuilder;

namespace .Application.DocumentTypes
{
    [DocumentType(AllowAtRoot=false)]
    public class DocumentBase : DocumentTypeBase
    {
public enum CmsTabs { Social = 49, Seo = 50, Details = 1, Content = 2, Navigation = 3, Materials = 4, Sizes = 5, Pictures = 6, Summary = 7, Header = 8, Map = 9, Settings = 99  }


        [DocumentTypeProperty(UmbracoPropertyType.Textstring, Tab = CmsTabs.Seo, Name = "SEO Title")]
        public string SeoTitle { get; set; }

        [DocumentTypeProperty(UmbracoPropertyType.TextboxMultiple, Tab = CmsTabs.Seo, Name = "SEO Keywords")]
        public string SeoKeywords { get; set; }

        [DocumentTypeProperty(UmbracoPropertyType.TextboxMultiple, Tab = CmsTabs.Seo, Name = "SEO Description")]
        public string SeoDescription { get; set; }

        [DocumentTypeProperty(UmbracoPropertyType.Textstring, Tab = CmsTabs.Social, Name = "Social Title")]
        public string SocialTitle { get; set; }

        [DocumentTypeProperty(UmbracoPropertyType.TextboxMultiple, Tab = CmsTabs.Social, Name = "Social Description")]
        public string SocialDescription { get; set; }

        [DocumentTypeProperty(UmbracoPropertyType.MediaPicker, Tab = CmsTabs.Social, Name = "Social Image")]
        public string SocialImage { get; set; }

        [DocumentTypeProperty(UmbracoPropertyType.TrueFalse, Name = "Hide In Navigation")]
        public string UmbracoNaviHide { get; set; }

        [DocumentTypeProperty(UmbracoPropertyType.ContentPicker, Name = "Redirect")]
        public string UmbracoRedirect { get; set; }
    }
}
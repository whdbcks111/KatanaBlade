using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vega.USiteBuilder;

namespace $rootnamespace$.Application.DocumentTypes
{
    [DocumentType(AllowedTemplates = new string[] { "ContentPage", "Search" }, AllowedChildNodeTypes = new Type[] { typeof(ContentPage) }, IconUrl = "icon-umb-content", AllowAtRoot=false)]
    public class ContentPage : DocumentBase
    {
        [DocumentTypeProperty(UmbracoPropertyType.Other, OtherTypeName = "Grid - Basic", Tab = CmsTabs.Details, Name = "Content")]
        public string Content { get; set; }
    }
}
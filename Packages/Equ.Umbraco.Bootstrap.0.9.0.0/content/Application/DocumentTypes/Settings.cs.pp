using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Vega.USiteBuilder;
using $rootnamespace$.Application.DocumentTypes;
using $rootnamespace$.Application.Converter;

namespace $rootnamespace$.Application.DocumentTypes
{
    [DocumentType(IconUrl = "icon-settings-alt-2", AllowAtRoot=true)]
    public class Settings : DocumentTypeBase
    {
        //navigation

        //[DocumentTypeProperty(UmbracoPropertyType.Other, OtherTypeName = "Multiple Content Picker", Tab = DocumentBase.CmsTabs.Navigation, Name = "Top Navigation", CustomTypeConverter = typeof(MultiNodeTreePickerConverter))]
        public List<IPublishedContent> TopNavigation { get; set; }

        //[DocumentTypeProperty(UmbracoPropertyType.Other, OtherTypeName = "Multiple Content Picker", Tab = DocumentBase.CmsTabs.Navigation, Name = "Utility Navigation", CustomTypeConverter = typeof(MultiNodeTreePickerConverter))]
        //public List<IPublishedContent> UtilityNavigation { get; set; }

        //[DocumentTypeProperty(UmbracoPropertyType.Other, OtherTypeName = "Multiple Content Picker", Tab = DocumentBase.CmsTabs.Navigation, Name = "Footer Navigation - Upper", CustomTypeConverter = typeof(MultiNodeTreePickerConverter))]
        //public List<IPublishedContent> FooterNavigation { get; set; }

        public static Settings Current
        {
            get
            {
                return ContentHelper.GetByNodeId<Settings>(1070);
            }
        }


    }
}
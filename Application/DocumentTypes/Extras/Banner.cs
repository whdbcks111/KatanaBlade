using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vega.USiteBuilder;

namespace .Application.DocumentTypes.Extras
{
    [DocumentType(IconUrl = "icon-umb-content", AllowAtRoot = false)]
    public class Banner : Extras
    {
        //[DocumentTypeProperty(UmbracoPropertyType.Other, OtherTypeName = "Banner Image Picker", Tab = DocumentBase.CmsTabs.Details, Name = "Picture", Mandatory = true)]
       // public string Picture { get; set; }

        //[DocumentTypeProperty(UmbracoPropertyType.Textstring, Tab = DocumentBase.CmsTabs.Details, Name = "Headline")]
        //public String Tagline { get; set; }

        //[DocumentTypeProperty(UmbracoPropertyType.Other, OtherTypeName = "Page Picker", Tab = DocumentBase.CmsTabs.Details, Name = "Link")]
        //public string Link { get; set; }


        public static List<Banner> GetList()
        {
            var parent = ContentHelper.GetByNode(umbraco.uQuery.GetNodesByType("BannerFolder").FirstOrDefault());
            var list = new List<Banner>();

            foreach (var item in parent.GetChildren())
            {
                list.Add(ContentHelper.GetByNodeId<Banner>(item.Id));
            }

            return list;
        }
    }
}

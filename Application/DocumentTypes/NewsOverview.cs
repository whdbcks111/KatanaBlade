using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vega.USiteBuilder;

namespace .Application.DocumentTypes
{
    [DocumentType(AllowedTemplates = new string[] { "NewsOverview" }, AllowedChildNodeTypes = new Type[] { typeof(News) }, IconUrl = "icon-umb-content", AllowAtRoot=false)]
    public class NewsOverview : DocumentBase
    {
       
    }
}
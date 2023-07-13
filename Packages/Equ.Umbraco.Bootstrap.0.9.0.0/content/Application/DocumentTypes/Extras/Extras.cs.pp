using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vega.USiteBuilder;

namespace $rootnamespace$.Application.DocumentTypes.Extras
{
    [DocumentType(AllowedChildNodeTypes = new Type[] { 
         typeof(BannerFolder)
    }, IconUrl = "icon-brick", AllowAtRoot=true)]
    public class Extras : DocumentTypeBase
    {

    }
}
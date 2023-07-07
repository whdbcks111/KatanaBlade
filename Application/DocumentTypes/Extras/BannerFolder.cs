using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vega.USiteBuilder;

namespace .Application.DocumentTypes.Extras
{
    [DocumentType(AllowedChildNodeTypes = new Type[] { 
         typeof(Banner)
    }, IconUrl = "icon-folder-outline", AllowAtRoot = false)]
    public class BannerFolder : Extras
    {

    }
}

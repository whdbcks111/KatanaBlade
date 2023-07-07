using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vega.USiteBuilder;


namespace .Application.DocumentTypes
{
    [DocumentType(AllowedTemplates = new string[] { "Homepage" }, AllowedChildNodeTypes = new Type[] { 
	typeof(ContentPage), 
	typeof(NewsOverview),
    	typeof(.Application.DocumentTypes.Extras.Extras),
   	 typeof(Settings)  	
    }, IconUrl = "icon-home", AllowAtRoot=true)]
    public class Homepage : DocumentBase
    {
    }
}
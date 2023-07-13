using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace .App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
			//force API to return json not xml
            configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
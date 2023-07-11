using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace $rootnamespace$.Application.HttpHandlers
{
    public class RobotsHttpHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";


            context.Response.Write("# Exclude Files From All Robots:\n");
            context.Response.Write("User-agent: *\n");
            context.Response.Write("Allow: /\n");
            context.Response.Write("Disallow: /aspnet_client/\n");
            context.Response.Write("Disallow: /bin/\n");
            context.Response.Write("Disallow: /config/\n");
            context.Response.Write("Disallow: /css/\n");
            context.Response.Write("Disallow: /macroScripts/\n");
            context.Response.Write("Disallow: /scripts/\n");
            context.Response.Write("Disallow: /umbraco/\n");
            context.Response.Write("Disallow: /umbraco_client/\n");
            context.Response.Write("Disallow: /usercontrols/\n");
            context.Response.Write("Disallow: /xslt/\n");
            context.Response.Write("# Optional sitemap URL:\n");
            context.Response.Write(String.Format("Sitemap: http://{0}/Sitemap.xml", context.Request.Url.Host));
        }
    }
}
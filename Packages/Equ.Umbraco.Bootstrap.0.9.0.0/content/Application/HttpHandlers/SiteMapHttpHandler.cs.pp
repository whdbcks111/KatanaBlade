using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Linq;
using umbraco;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Configuration;
using Umbraco.Web;

namespace $rootnamespace$.Application.HttpHandlers
{
    public class SiteMapHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(((object)context.Request.RawUrl).ToString())))
                this.StreamExistingFile(context);
            else
                this.GenerateSiteMap(context);
        }

        private void GenerateSiteMap(HttpContext context)
        {
            string str1 = context.Request.Url.PathAndQuery.ToLower().Replace(context.Request.ApplicationPath.ToLower(), "");
            string str2 = context.Request.Url.AbsoluteUri.ToLower().Replace(str1.ToLower(), "");
            UmbracoContext.EnsureContext(new HttpContextWrapper(HttpContext.Current), ApplicationContext.Current);
            var contentService = ApplicationContext.Current.Services.ContentService;
            var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);


            if (!str2.EndsWith("/"))
                str2 = str2 + "/";
            HttpResponse response = context.Response;
            response.ContentType = "text/xml";
            using (TextWriter w = (TextWriter)new StreamWriter(response.OutputStream, Encoding.UTF8))
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.WriteStartDocument();
                ((XmlWriter)xmlTextWriter).WriteStartElement("urlset");
                xmlTextWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xmlTextWriter.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
                xmlTextWriter.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                var rootNode = umbracoHelper.TypedContent(umbracoHelper.ContentAtRoot()[0].Id);
               
                if (rootNode != null)
                {
                    bool flag = true;
                    foreach (SiteMapItem siteMapItem in SiteMapItem.GetAll(rootNode))
                    {
                        if (flag && str2.EndsWith("/") && siteMapItem.Url.StartsWith("/"))
                        {
                            str2 = str2.Substring(0, str2.Length - 1);
                            flag = false;
                        }
                        ((XmlWriter)xmlTextWriter).WriteStartElement("url");
                        xmlTextWriter.WriteElementString("loc", str2 + siteMapItem.Url);
                        xmlTextWriter.WriteElementString("lastmod", siteMapItem.LastModified);
                        if (!string.IsNullOrEmpty(siteMapItem.ChangeFrequency))
                            xmlTextWriter.WriteElementString("changefreq", siteMapItem.ChangeFrequency);
                        if (!string.IsNullOrEmpty(siteMapItem.Priority))
                            xmlTextWriter.WriteElementString("priority", siteMapItem.Priority);
                        xmlTextWriter.WriteEndElement();
                    }
                }
                xmlTextWriter.WriteEndElement();
            }
        }

        private void StreamExistingFile(HttpContext context)
        {
            HttpResponse response = context.Response;
            string filename = HttpContext.Current.Server.MapPath(((object)context.Request.RawUrl).ToString());
            string str = string.Empty;
            response.ContentType = "text/xml";
            response.TransmitFile(filename);
        }
    }
}
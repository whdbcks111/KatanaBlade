using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web;
using Umbraco.Core.Models;
using System.Text.RegularExpressions;
using $rootnamespace$.Models.Parts;
using HtmlAgilityPack;

namespace $rootnamespace$.Application.Extensions
{
    public static class UmbracoExtension
    {
		/// <summary>
        /// Returns Youtube Id based on url
        /// </summary>
        /// <param name="itemsPerPage"></param>
        /// <param name="numberOfItems"></param>
        /// <returns></returns>
        public static string GetYouTubeId(this Umbraco.Web.UmbracoHelper umbraco, string url)
        {
            return Regex.Match(url, @"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&amp;]v=)|youtu\.be\/)([^""&amp;?\/ ]{11})").Groups[1].Value;
        }
		
        /// <summary>
        /// Return all fields required for paging.
        /// </summary>
        /// <param name="itemsPerPage"></param>
        /// <param name="numberOfItems"></param>
        /// <returns></returns>
        public static Pager GetPager(this UmbracoHelper umbraco, int itemsPerPage, int numberOfItems)
        {
            // paging calculations
            int currentPage;
            if (!int.TryParse(HttpContext.Current.Request.QueryString["page"], out currentPage))
            {
                currentPage = 1;
            }

            var numberOfPages = numberOfItems % itemsPerPage == 0 ? Math.Ceiling((decimal)(numberOfItems / itemsPerPage)) : Math.Ceiling((decimal)(numberOfItems / itemsPerPage)) + 1;
            var pages = Enumerable.Range(1, (int)numberOfPages);

            return new Pager()
            {
                NumberOfItems = numberOfItems,
                ItemsPerPage = itemsPerPage,
                CurrentPage = currentPage,
                Pages = pages
            };
        }
		
		/// <summary>
        /// Return a string of number texts (bytes) and convert to Mega Bytes
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        public static string GetSizeText(this UmbracoHelper umbraco, IPublishedContent media)
        {
            try
            {
                var bytesStr = media.GetPropertyValue("umbracoBytes").ToString();
                int bytes = 0;

                int.TryParse(bytesStr, out bytes);

                Double kb = Convert.ToDouble(bytes / 1024);
                Double mb = Convert.ToDouble(bytes / 1024 / 1024);

                if (mb > 0.0)
                {
                    return mb.ToString("#.##") + "MB";
                }
                else if (kb > 0.0)
                {
                    return kb.ToString("#.##") + "Kb";
                }
                else
                {
                    return bytes + " bytes";
                }

            }
            catch (Exception)
            { }

            return "";
        }
		
		public static string RenderMacroContent(this UmbracoHelper umbraco, string rteContent)
        {
            if (string.IsNullOrEmpty(rteContent))
            {
                return string.Empty;
            }

            var html = new HtmlDocument();
            html.LoadHtml(rteContent);

            //get all the comment nodes we want
            var commentNodes = html.DocumentNode.SelectNodes("//comment()[contains(., '<?UMBRACO_MACRO')]");
            if (commentNodes == null)
            {
                //There are no macros found, just return the normal content
                return rteContent;
            }

            //replace each containing parent <div> with the comment node itself. 
            foreach (var c in commentNodes)
            {
                var div = c.ParentNode;
                var divContainer = div.ParentNode;
                divContainer.ReplaceChild(c, div);
            }

            var parsed = html.DocumentNode.OuterHtml;

            Regex MacroRteContent = new Regex(@"(<!--\s*?)(<\?UMBRACO_MACRO.*?/>)(\s*?-->)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);


            //now replace all the <!-- and --> with nothing
            return global::umbraco.library.RenderMacroContent(MacroRteContent.Replace(parsed, match =>
            {
                if (match.Groups.Count >= 3)
                {
                    //get the 3rd group which is the macro syntax
                    return match.Groups[2].Value;
                }
                //replace with nothing if we couldn't find the syntax for whatever reason
                return string.Empty;
            }), UmbracoContext.Current.PageId.Value);
        }

    }
}
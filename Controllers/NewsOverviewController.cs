using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Umbraco.Web;
using Umbraco.Web.Models;
using DevTrends.MvcDonutCaching;
using .Application.DocumentTypes;
using .Models;
using .Application.Extensions;

namespace .Controllers
{
    public class NewsOverviewController : Umbraco.Web.Mvc.RenderMvcController
    {
        public ActionResult Index(RenderModel model)
        {
            var newsList = News.GetNewsList();
            var pager = Umbraco.GetPager(5, newsList.Count());

            var newsOverviewViewModel = new NewsOverviewViewModel()
            {
                NewsList = newsList.Skip((pager.CurrentPage - 1) * pager.ItemsPerPage).Take(pager.ItemsPerPage),
                Pager = pager
            };

            return CurrentTemplate(newsOverviewViewModel);
        }
    }
}
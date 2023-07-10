using DevTrends.MvcDonutCaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using $rootnamespace$.Controllers.Base;
using $rootnamespace$.Models;

namespace $rootnamespace$.Controllers
{

    public class DefaultController : BaseSurfaceController
    {
        /// <summary>
        /// If the route hijacking doesn't find a controller this default controller will be used.
        /// That way a each page will always go through a controller and we can always have a BaseModel for the masterpage.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override ActionResult Index(RenderModel model)
        {
            var baseModel = new BaseModel();
            return CurrentTemplate(baseModel);
        }
    }
}
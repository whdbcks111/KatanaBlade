using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using System.Web.Optimization;
using .Application;

namespace .App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundleCollection)
        {
            //var cssTransformer = new StyleTransformer();
            bundleCollection.UseCdn = true;

            var nullBuilder = new NullBuilder();
            var nullOrderer = new NullOrderer();

            var mainlessBundle = new CustomStyleBundle("~/bundles/site-less");
            
			mainlessBundle.IncludeDirectory("~/css/site", "*.less", false);
            mainlessBundle.Include("~/css/bootstrap.less");
            mainlessBundle.Orderer = nullOrderer;
            mainlessBundle.Transforms.Add(new CssMinify());
            bundleCollection.Add(mainlessBundle);

            //Modernizr
            var modernizrBundle = new CustomScriptBundle("~/bundles/modernizr");
            modernizrBundle.Include("~/scripts/modernizr-*");
            modernizrBundle.Orderer = nullOrderer;
            bundleCollection.Add(modernizrBundle);

			var oldieBundle = new CustomScriptBundle("~/bundles/oldie");
            oldieBundle.Include("~/scripts/respond.min.js");
            oldieBundle.Orderer = nullOrderer;
            bundleCollection.Add(oldieBundle);
			
            //JS
            var mainjsBundle = new CustomScriptBundle("~/bundles/site-js");
			mainjsBundle.IncludeDirectory("~/scripts/site", "*.js", false);

            mainjsBundle.Include(
               "~/scripts/bootstrap.min.js",
               "~/scripts/underscore-min.js",
			   "~/scripts/imagesloaded.pkgd.min.js",
			   "~/scripts/jquery.matchHeight-min.js",
			   // This should be loaded last as it will setup all the javascript
                "~/scripts/controller.js"
             );
            mainjsBundle.Orderer = nullOrderer;
            bundleCollection.Add(mainjsBundle);


            //Jquery
            var jqueryBundle = new CustomScriptBundle("~/bundles/jquery"); //, "//ajax.googleapis.com/ajax/libs/jquery/{version}/jquery.min.js");
            jqueryBundle.Include("~/scripts/jquery-{version}.js");
            jqueryBundle.CdnFallbackExpression = "window.jQuery";
            jqueryBundle.Orderer = nullOrderer;
            bundleCollection.Add(jqueryBundle);
        }
    }
}
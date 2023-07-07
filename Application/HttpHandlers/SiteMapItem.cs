using System.Collections;
using System.Collections.Generic;
using umbraco.presentation.nodeFactory;
using Umbraco.Core.Models;
using System.Linq;

namespace .Application.HttpHandlers
{
    public class SiteMapItem
    {
        private string _url;
        private string _lastModified;
        private string _changeFrequency;
        private string _priority;

        public string Url
        {
            get
            {
                return this._url;
            }
        }

        public string LastModified
        {
            get
            {
                return this._lastModified;
            }
        }

        public string ChangeFrequency
        {
            get
            {
                return this._changeFrequency;
            }
        }

        public string Priority
        {
            get
            {
                return this._priority;
            }
        }

        public SiteMapItem(string url, string lastModified, string changeFrequency, string priority)
        {
            this._url = url;
            this._lastModified = lastModified;
            this._changeFrequency = changeFrequency;
            this._priority = priority;
        }

        public static List<SiteMapItem> GetAll(IPublishedContent rootNode)
        {
            return SiteMapItem.GetAllChildren(rootNode, true);
        }

        public static List<SiteMapItem> GetAllChildren(IPublishedContent node, bool addParentNode)
        {
            List<SiteMapItem> list = new List<SiteMapItem>();
            if (addParentNode)
            {
                SiteMapItem siteMapItem = SiteMapItem.NodeToSiteMapItem(node);
                if (siteMapItem != null)
                    list.Add(siteMapItem);
            }
            foreach (var node1 in node.Children)
            {
                SiteMapItem siteMapItem1 = SiteMapItem.NodeToSiteMapItem(node1);
                if (siteMapItem1 != null)
                    list.Add(siteMapItem1);
                foreach (SiteMapItem siteMapItem2 in SiteMapItem.GetAllChildren(node1, false))
                    list.Add(siteMapItem2);
            }
            return list;
        }

        public static SiteMapItem NodeToSiteMapItem(IPublishedContent node)
        {
            if (!(SiteMapItem.GetSafePropertyValue(node, "umbracoNaviHide") != "1"))
                return (SiteMapItem)null;
            string changeFrequencyValue = SiteMapItem.GetChangeFrequencyValue(node, "seSitemapChangeFreq");
            string priorityValue = SiteMapItem.GetPriorityValue(node, "seSitemapPriority");
            string lastModified = node.UpdateDate.ToString("yyyy-MM-dd");
            return new SiteMapItem(node.Url, lastModified, changeFrequencyValue, priorityValue);
        }

        public static string GetChangeFrequencyValue(IPublishedContent n, string propertyAlias)
        {
            string[] strArray = new string[7]
	  {
		"always",
		"hourly",
		"daily",
		"weekly",
		"monthly",
		"yearly",
		"never"
	  };
            string str1 = SiteMapItem.GetSafePropertyValue(n, propertyAlias).Trim().ToLower();
            if (!string.IsNullOrEmpty(str1))
            {
                foreach (string str2 in strArray)
                {
                    if (str1.Equals(str2))
                        return str1;
                }
            }
            return string.Empty;
        }

        public static string GetPriorityValue(IPublishedContent n, string propertyAlias)
        {
            string[] strArray = new string[11]
	  {
		"0.0",
		"0.1",
		"0.2",
		"0.3",
		"0.4",
		"0.5",
		"0.6",
		"0.7",
		"0.8",
		"0.9",
		"1.0"
	  };
            string str1 = SiteMapItem.GetSafePropertyValue(n, propertyAlias).Trim().ToLower();
            if (!string.IsNullOrEmpty(str1))
            {
                foreach (string str2 in strArray)
                {
                    if (str1.Equals(str2))
                        return str1;
                }
            }
            return string.Empty;
        }

        public static string GetSafePropertyValue(IPublishedContent n, string propertyAlias)
        {
            if (n.GetProperty(propertyAlias) != null && !string.IsNullOrEmpty(n.GetProperty(propertyAlias).Value.ToString()))
                return n.GetProperty(propertyAlias).Value.ToString();
            else
                return string.Empty;
        }
    }
}
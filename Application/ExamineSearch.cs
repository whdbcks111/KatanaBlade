using Examine;
using Examine.SearchCriteria;
using System;
using System.Collections.Generic;
using Umbraco.Web;
using Examine.LuceneEngine.SearchCriteria;
using Lucene.Net.Search;
using Umbraco.Core.Models;
using System.Linq;
using Umbraco.Core.Logging;
using Vega.USiteBuilder;
using Lucene.Net.Analysis.Standard;

namespace .Application
{
    /// <summary>
    ///
    /// To use this class 
    /// 1) Add node name search field to ExamineIndex.config .... eg. <add Name="genreSearch" />;
    /// 2) Add field type to fields in Application Startup ... eg. string[] fields = { "residentCompany", "genre" };
    /// 3) Add EventHanlder to Application Startup ... eg. EventIndexerSite.GatheringNodeData += new EventHandler<IndexingNodeDataEventArgs>(EventIndexer_AddSearchFields);
    /// 4) Add 'ExamineSearch.ModifyNodeDataForFields(fields, e);' call to 'EventIndexer_AddSearchFields' in Application Startup
    /// 
    /// </summary>
    public class ExamineSearch
    {
        /// <summary>
        /// Search document for a containing id.
        /// This is used when there is a single id saved to examine.
        /// </summary>
        /// <param name="alias">Alias of property</param>
        /// <param name="idList">List of id's</param>
        /// <returns></returns>
        public static String IdQuery(String alias, IEnumerable<int> idList)
        {
            String luceneString = "";

            if (idList != null && idList.Count() > 0)
            {
                luceneString += "+(";

                foreach (int i in idList)
                {
                    luceneString += alias + ":" + i.ToString() + " ";
                }

                luceneString += ") ";
            }

            return luceneString;
        }

        /// <summary>
        /// Search through a list of id's supplied in examine for another list.
        /// This is used when there is a csv saved to the database. 
        /// </summary>
        /// <param name="alias">Name of property</param>
        /// <param name="idList">List of id's to search for</param>
        /// <returns></returns>
        public static String ListQuery(String alias, IEnumerable<int> idList)
        {
            String luceneString = "";

            if (idList != null && idList.Count() > 0)
            {
                luceneString += "+(";

                foreach (int i in idList)
                {
                    luceneString += alias + "Search:liststart*aa" + i.ToString() + "zz*listend ";
                }

                luceneString += ") ";
            }

            return luceneString;
        }

        /// <summary>
        /// Search through a list of id's supplied in examine for another list.
        /// This is used when there is a csv saved to the database. 
        /// </summary>
        /// <param name="alias">Name of property</param>
        /// <param name="idList">List of id's to search for</param>
        /// <returns></returns>
        public static String ListStringQuery(String alias, IEnumerable<string> idList)
        {
            String luceneString = "";

            if (idList != null && idList.Count() > 0)
            {
                luceneString += "+(";

                foreach (string i in idList)
                {
                    luceneString += alias + "Search:liststart*aa" + i.ToString() + "zz*listend ";
                }

                luceneString += ") ";
            }

            return luceneString;
        }

        /// <summary>
        /// Search a examine for a list of dates.
        /// This is used when a list of dates is being saved do examine.
        /// </summary>
        /// <param name="alias">Alias of property</param>
        /// <param name="dateList">List of dates</param>
        /// <returns></returns>
        public static String DatesQuery(String alias, List<DateTime> dateList )
        {
            String luceneString = "";

            if (dateList != null)
            {
                luceneString += "+(";

                foreach (DateTime d in dateList)
                {
                    String strDate = "" + String.Format("{0:D4}", d.Year) + "-" + String.Format("{0:D2}", d.Month) + "-" + String.Format("{0:D2}", d.Day) + "";
                    luceneString += alias + ":*" + strDate + "* ";
                }

                luceneString += ") ";
            }

            return luceneString;
        }

        public static String SearchTermQuery(string searchTerm)
        {
            String luceneString = "";

            if (searchTerm != null && searchTerm.Trim().Length > 0)
            {
                searchTerm = searchTerm.Replace(".", "");
                String orgSearchTerm = searchTerm;
                //searchTerm if doesnt start with * put *searchTerm if doest with * 
                if (searchTerm.IndexOf("*") != 0)
                {
                    searchTerm = "*" + searchTerm;
                }
                if (searchTerm.LastIndexOf("*") != searchTerm.Length)
                {
                    searchTerm = searchTerm + "*";
                }

                luceneString += "+(";
                luceneString += "nodeName:(+" + searchTerm.Replace(" ", " +") + ")^5 "; // Boost(5) for nodeName that contains ALL search terms
                luceneString += "nodeName:" + searchTerm + " ";
                luceneString += "content:(+" + searchTerm.Replace(" ", " +") + ")^3 "; // Boost(3) for content that contains ALL search terms
                luceneString += "content:" + searchTerm + " ";
                luceneString += "description:(+" + searchTerm.Replace(" ", " +") + ")^2 "; // Boost(2) for description that contains ALL search terms
                luceneString += "description:" + searchTerm + " ";
                luceneString += ") ";
            }

            return luceneString;
        }

        public static IEnumerable<SearchResult> ContentSearch(string nodeTypeAlias, String searchTerm = null)
        {
            //Fetching our SearchProvider by giving it the name of our searchprovider 
            var Searcher = Examine.ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
            var searchCriteria = Searcher.CreateSearchCriteria(BooleanOperation.And);

            var luceneString = "+(nodeTypeAlias: " + nodeTypeAlias + ") ";
            luceneString += SearchTermQuery(searchTerm);

            var query = searchCriteria.RawQuery(luceneString);
            return Searcher.Search(query).OrderByDescending(x => x.Score);
        }

        public static IEnumerable<SearchResult> FaqSearch(IEnumerable<int> faqType = null, IEnumerable<string> faqTopic = null, string searchTerm = "")
        {
            //Fetching our SearchProvider by giving it the name of our searchprovider 
            var Searcher = Examine.ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
            var searchCriteria = Searcher.CreateSearchCriteria(BooleanOperation.And);

            var luceneString = "+(nodeTypeAlias: Faq) ";
            luceneString += SearchTermQuery(searchTerm);
            luceneString += ListQuery("faqType", faqType);
            luceneString += ListStringQuery("topic", faqTopic);

            var query = searchCriteria.RawQuery(luceneString);
            return Searcher.Search(query).OrderByDescending(x => x.Score);
        }



        public static void ModifyNodeDataForFields(string[] fields, IndexingNodeDataEventArgs e)
        {
            foreach (string fieldName in fields)
            {
                try
                {
                    if (e.Fields.ContainsKey(fieldName))
                    {
                        var fieldNodes = e.Fields[fieldName].Split(',');
                        var returnString = "liststart";

                        foreach (var id in fieldNodes)
                        {
                            if (string.IsNullOrWhiteSpace(id) == false)
                            {
                                returnString += "aa" + id + "zz";
                            }
                        }

                        returnString += "listend";
                        e.Fields.Add(fieldName + "Search", returnString);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(typeof(ExamineSearch), "Error in ModifyNodeDataForFields", ex);
                }
            }
        }

        public static void ModifyNodeDataForNumericFields(string[] fields, IndexingNodeDataEventArgs e)
        {
            foreach (string fieldName in fields)
            {
                try
                {
                    var fieldNodes = e.Fields[fieldName].Split(',');
                    int returnInt = 0;

                    foreach (var id in fieldNodes)
                    {
                        int.TryParse(id, out returnInt);
                    }

                    e.Fields.Add(fieldName + "Int", returnInt.ToString("D5"));
                }
                catch (Exception ex)
                {
                    LogHelper.Error(typeof(ExamineSearch), "Error in ModifyNodeDataForNumericFields", ex);
                }
            }
        }
    }

    public static class ExamineSearchListExtension
    {
        public static List<T> ConvertToContent<T>(this IEnumerable<SearchResult> lstObj)
        {
            List<T> lstObjects = new List<T>();

            foreach (var obj in lstObj)
            {
                DocumentTypeBase objDoc = ContentHelper.GetByNodeId(obj.Id);
                var objTyped = (new[] { objDoc }).Cast<T>().Single();

                if (objTyped != null)
                {
                    lstObjects.Add(objTyped);
                }
            }

            return lstObjects;
        }
    }
}
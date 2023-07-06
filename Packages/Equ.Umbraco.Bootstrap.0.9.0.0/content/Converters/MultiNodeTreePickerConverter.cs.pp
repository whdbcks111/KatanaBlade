using System;
using System.Collections.Generic;
using Vega.USiteBuilder.Types;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Core.Dynamics;


namespace $rootnamespace$.Application.Converter
{
    public class MultiNodeTreePickerConverter : ICustomTypeConvertor
    {
        public Type ConvertType
        {
            get
            {
                return typeof(List<IPublishedContent>);
            }
        }

        public object ConvertValueWhenRead(object inputValue)
        {
            try
            {
                List<IPublishedContent> retVal = new List<IPublishedContent>();

                if (!string.IsNullOrEmpty(inputValue.ToString()))
                {
                    UmbracoHelper helper = new UmbracoHelper(UmbracoContext.Current);
                    // xml
                    if (inputValue.ToString().Contains("</MultiNodePicker>"))
                    {
                        foreach (dynamic item in new DynamicXml(inputValue.ToString()))
                        {
							try
                            {
								retVal.Add(helper.TypedContent(item.BaseElement.Value));
							}
							catch(Exception)
                            {
                            }
                        }
                    }
                    // csv
                    else
                    {
                        foreach (var nodeId in inputValue.ToString().Split(','))
                        {
							try{
								retVal.Add(helper.TypedContent(nodeId));
							}
							catch(Exception)
                            {
                            }
                        }
                    }
                }
                return retVal;
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Cannot convert '{0}' to List<DynamicNode>. Error: {1}", inputValue.ToString(), exc.Message));
            }
        }

        public object ConvertValueWhenWrite(object inputValue)
        {
            throw new NotImplementedException();
        }
    }
}
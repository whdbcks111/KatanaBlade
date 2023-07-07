using System;
using System.Collections.Generic;
using Vega.USiteBuilder.Types;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Core.Dynamics;
using umbraco;
using Vega.USiteBuilder;

namespace .Application.Converter
{
    public class TypedContentListConverter : ICustomTypeConvertor
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
            var returnList = new List<IPublishedContent>();
            if (inputValue != null && !string.IsNullOrEmpty(inputValue.ToString()))
            {
                var service = new UmbracoHelper(UmbracoContext.Current);
                foreach (var id in inputValue.ToString().Split(','))
                {
                    returnList.Add(service.TypedContent(id));
                }
            }
            return returnList;
        }

        public object ConvertValueWhenWrite(object inputValue)
        {
            throw new NotImplementedException();
        }
    }
}
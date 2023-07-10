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
    public class TypedMediaConverter : ICustomTypeConvertor
    {
        public Type ConvertType
        {
            get
            {
                return typeof(IPublishedContent);
            }
        }

        public object ConvertValueWhenRead(object inputValue)
        {
            if (inputValue != null && !string.IsNullOrEmpty(inputValue.ToString()))
            {
                try
                {
                    var service = new UmbracoHelper(UmbracoContext.Current);
                    return service.TypedMedia(inputValue.ToString());
                }
                catch (Exception) { }
            }
            return null;
        }

        public object ConvertValueWhenWrite(object inputValue)
        {
            throw new NotImplementedException();
        }
    }
}
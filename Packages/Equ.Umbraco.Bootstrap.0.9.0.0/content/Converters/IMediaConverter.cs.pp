using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vega.USiteBuilder.Types;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core;

namespace $rootnamespace$.Application.Converters
{
    public class IMediaConverter : ICustomTypeConvertor
    {
        public Type ConvertType
        {
            get
            {
                return typeof(IMedia);
            }
        }

        public object ConvertValueWhenRead(object inputValue)
        {
            if (inputValue != null && !string.IsNullOrEmpty(inputValue.ToString()))
            {
                var service = ApplicationContext.Current.Services.MediaService;

                return service.GetById(int.Parse(inputValue.ToString()));
            }
            return null;
        }

        public object ConvertValueWhenWrite(object inputValue)
        {
            throw new NotImplementedException();
        }
    }
}

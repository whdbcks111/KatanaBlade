using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using $rootnamespace$.Application.DocumentTypes;
using $rootnamespace$.Models.Parts;

namespace $rootnamespace$.Models
{
    public class NewsOverviewViewModel : BaseModel
    {
        public IEnumerable<News> NewsList;
        public Pager Pager;
    }
}
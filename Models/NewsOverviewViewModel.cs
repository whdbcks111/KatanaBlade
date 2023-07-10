using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using .Application.DocumentTypes;
using .Models.Parts;

namespace .Models
{
    public class NewsOverviewViewModel : BaseModel
    {
        public IEnumerable<News> NewsList;
        public Pager Pager;
    }
}
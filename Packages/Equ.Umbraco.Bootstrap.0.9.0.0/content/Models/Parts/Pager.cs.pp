﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $rootnamespace$.Models.Parts
{
    public class Pager
    {
        public Pager()
        {
        }

        public int NumberOfItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<int> Pages { get; set; }

        public bool IsFirstPage
        {
            get
            {
                return CurrentPage <= 1;
            }
        }

        public bool IsLastPage
        {
            get
            {
                return (CurrentPage * ItemsPerPage) >= NumberOfItems;
            }
        }
    }
}
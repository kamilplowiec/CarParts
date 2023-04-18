using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CarParts.Models
{
    public class TableResult<T>
    {
        public int TotalRecordCount { get; set; }
        public IList<T> Records { get; set; }
        public string Result { get { return "OK"; } }
    }
}
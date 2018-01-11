using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSBXG.Models
{
    public class TabContent
    {
        public string Type { get; set; }
        public string Category { get; set; }
        public string Text { get; set; }
        public IList<Document> Content { get; set; }
        public bool ItemWithDate { get; set; }
        public bool ItemWithTime { get; set; }
        public bool ItemWithLink { get; set; }
    }
}
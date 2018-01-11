using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSBXG.Models
{
    public class PermissionData
    {
        public string Category { get; set; }
        public string SitemapId { get; set; }
        public string Text { get; set; }
        public bool Access { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TSBXG.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Document
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public string Html { get; set; }
        public string Tag { get; set; }
        public byte[] AttachFile { get; set; }
        public string AttachFileName { get; set; }
        public string AttachFileType { get; set; }
        public string AttachFileUrl { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public string CreateIp { get; set; }
        public string UpdateIp { get; set; }
        public int Hits { get; set; }
    
        public virtual Sitemap Sitemap { get; set; }
    }
}

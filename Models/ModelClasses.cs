using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSBXG.Models
{
    [MetadataType(typeof(MetadataDocument))]
    public partial class Document
    {

    }
    public class MetadataDocument
    {
        [Display(Name = "主键")]
        public object Id { get; set; }

        [Display(Name = "分类")]
        public object Category { get; set; }

        [Display(Name = "标题")]
        public object Title { get; set; }

        [Display(Name = "图像数据")]
        public object ImageData { get; set; }

        [Display(Name = "图像类型")]
        [HiddenInput(DisplayValue = false)]
        public object ImageMimeType { get; set; }

        [Display(Name = "HTML文本")]
        public object Html { get; set; }

        [Display(Name = "标记")]
        public string Tag { get; set; }

        [Display(Name = "创建时间")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        [Display(Name = "更新时间")]
        public Nullable<System.DateTime> UpdateTime { get; set; }

        [Display(Name = "文件")]
        public byte[] AttachFile { get; set; }

        [Display(Name = "文件类型")]
        public string AttachFileType { get; set; }

        [Display(Name = "文件地址")]
        public string AttachFileUrl { get; set; }

        [Display(Name = "站点地图")]
        public virtual Sitemap Sitemap { get; set; }
    }
    [MetadataType(typeof(MetadataUser))]
    public partial class User
    {

    }
    public class MetadataUser
    {
        [Display(Name = "主键")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "姓名")]
        public string FullName { get; set; }

        [Display(Name = "描述")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "权限")]
        public virtual ICollection<Permision> Permision { get; set; }
    }
    [MetadataType(typeof(MetadataSitemap))]
    public partial class Sitemap { }
    public class MetadataSitemap
    {
        [Required]
        [Display(Name = "主键")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "文本")]
        public string Text { get; set; }

        [Display(Name = "管理URL")]
        public string ManageUrl { get; set; }

        [Display(Name = "前台URL")]
        public string ViewUrl { get; set; }

        [Display(Name = "顺序")]
        public Nullable<decimal> DisplayIndex { get; set; }

        [Display(Name = "父对象ID")]
        public string ParentId { get; set; }

        public virtual ICollection<Document> Document { get; set; }
        public virtual ICollection<Permision> Permision { get; set; }
    }


    [MetadataType(typeof(MetadataOutLink))]
    public partial class OutLink { }
    public class MetadataOutLink
    {
        [Required]
        [Display(Name = "主键")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "文本")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "URL地址")]
        public string Url { get; set; }

        [Display(Name = "图标数据")]
        public byte[] Icon { get; set; }

        [Display(Name = "图标类型")]
        public string IconMimeType { get; set; }

        [Display(Name = "顺序")]
        public Nullable<decimal> DispalyIndex { get; set; }
    }
}
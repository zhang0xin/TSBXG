﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TSBXGContainer : DbContext
    {
        public TSBXGContainer()
            : base("name=TSBXGContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<OutLink> OutLink { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Sitemap> Sitemap { get; set; }
        public DbSet<Permision> Permision { get; set; }
        public DbSet<Document> Document { get; set; }
    }
}

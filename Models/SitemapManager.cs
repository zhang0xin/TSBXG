using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace TSBXG.Models
{
    public class SitemapManager
    {
        private static TSBXGContainer db = new TSBXGContainer();
        public static TreeNode<Sitemap> _sitemapTree;
        public static TreeNode<Sitemap> _viewSitemapTree;
        static SitemapManager()
        {

        }
        public static PermissionData[] GetPermissions(int userId)
        {
            var pl = new List<PermissionData>();
            var dics = GetLeafNodes();
            foreach (var key in dics.Keys)
            {
                if (String.IsNullOrWhiteSpace(dics[key].ManageUrl)) continue;
                if (dics[key].ViewNeedPermission == "1")
                {
                    pl.Add(new PermissionData() { SitemapId = dics[key].Id, Text = key, Access = false, Category = "前台" });
                }
                pl.Add(new PermissionData() { SitemapId = dics[key].Id, Text = key, Access = false, Category = "管理" });
            }
            var sids = (new TSBXGContainer()).User.Find(userId).Permision.Select(p => p.Tag + ":" + p.SitemapId);
            foreach (var p in pl)
            {
                p.Access = sids.Contains(p.Category + ":" + p.SitemapId);
            }
            return pl.ToArray();
        }
        public static Dictionary<string, Sitemap> GetLeafNodes()
        {
            var dicChilds = new Dictionary<string, Sitemap>();
            GetSitemapTree().Traverse(
                delegate(TreeNode<Sitemap> node)
                {
                    if (node.Children.Count > 0) return;
                    string path = node.Path(n => n.Value.Text);
                    dicChilds.Add(path, node.Value);
                });
            return dicChilds;
        }
        public static void InitTreeNode()
        {
            var sitemaps = db.Sitemap.ToList();
            _sitemapTree = new TreeNode<Sitemap>(sitemaps.FirstOrDefault(s => s.ParentId == null));

            FillTreeNode(_sitemapTree, sitemaps);
        }
        private static void FillTreeNode(TreeNode<Sitemap> parentNode, List<Sitemap> allNodes)
        {
            string parentId = parentNode.Value.Id;
            var nodes = allNodes.Where(s => s.ParentId == parentId).OrderBy(s => s.DisplayIndex).ToArray();
            for (int i = 0; i < nodes.Count(); i++)
            {
                var child = parentNode.AddChild(nodes[i]);
                FillTreeNode(child, allNodes);
            }
        }
        public static List<Sitemap> GetParentViewSitemapList(string category)
        {
            List<Sitemap> sitemapList = new List<Sitemap>();
            TreeNode<Sitemap> targetNode = null;
            var viewTree = GetViewSitemapTree();
            viewTree.Traverse(
                delegate(TreeNode<Sitemap> node)
                {
                    if (node.Value.Id == category)
                    {
                        targetNode = node;
                    }
                });
            for (; targetNode != null; )
            {
                sitemapList.Insert(0, targetNode.Value);
                targetNode = targetNode.Parent;
            }

            return sitemapList;
        }
        public static TreeNode<Sitemap> GetManageSitemapTree(int userid = 0)
        {
            var user = (new TSBXGContainer()).User.Find(userid);
            if (user == null) return null;
            if (user.UserName == "admin")
            {
                return GetSitemapTree().Filter(
                    node => !string.IsNullOrWhiteSpace(node.Value.ManageUrl), true);
            }

            var sitemapIds = user.Permision.Where(w => w.Tag == "管理").Select(p => p.SitemapId);
            var manageTree = GetSitemapTree().Filter(
                    node => sitemapIds.Contains(node.Value.Id) && !string.IsNullOrWhiteSpace(node.Value.ManageUrl), true);
            return manageTree;
        }
        public static TreeNode<Sitemap> GetViewSitemapTree()
        {
            if (_viewSitemapTree == null)
            {
                _viewSitemapTree = GetSitemapTree().Filter(
                    node => !string.IsNullOrWhiteSpace(node.Value.ViewUrl));
            }
            return _viewSitemapTree;
        }
        public static TreeNode<Sitemap> GetSitemapTree()
        {
            if (_sitemapTree == null) InitTreeNode();
            return _sitemapTree;
        }

        public static MvcHtmlString Test(Func<int, HelperResult> func)
        {
            string temp = "";
            for (int i = 0; i < 10; i++)
            {
                temp += func(i);
            }
            return MvcHtmlString.Create(temp);
        }
    }
}
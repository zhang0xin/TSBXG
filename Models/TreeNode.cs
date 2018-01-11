using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace TSBXG.Models
{
    public class TreeNode<T>
    {
        private readonly T _value;
        private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();

        public string Path(Func<TreeNode<T>, string> func, string separator = " | ")
        {
            var curr = this;
            string path = func(curr);
            while (curr.Parent != null)
            {
                path = func(curr.Parent) + separator + path;
                curr = curr.Parent;
            }
            return path;
        }

        public TreeNode(T value)
        {
            _value = value;
        }

        public TreeNode<T> this[int i]
        {
            get { return _children[i]; }
        }

        public TreeNode<T> Parent { get; private set; }

        public T Value { get { return _value; } }

        public ReadOnlyCollection<TreeNode<T>> Children
        {
            get { return _children.AsReadOnly(); }
        }

        private void AddChild(TreeNode<T> node)
        {
            node.Parent = this;
            _children.Add(node);
        }

        public TreeNode<T> AddChild(T value)
        {
            var node = new TreeNode<T>(value) { Parent = this };
            _children.Add(node);
            return node;
        }

        public TreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(TreeNode<T> node)
        {
            return _children.Remove(node);
        }

        public void Traverse(Action<TreeNode<T>> action)
        {
            action(this);
            foreach (var child in _children)
                child.Traverse(action);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in _children)
                child.Traverse(action);
        }

        public TreeNode<T> Filter(Func<TreeNode<T>, bool> check, bool requireParent = false)
        {
            var node = new TreeNode<T>(this.Value);
            foreach (var child in Children)
            {
                var childNode = child.Filter(check, requireParent);
                if (childNode != null)
                {
                    node.AddChild(childNode);
                }
            }
            if (requireParent && node.Children.Count > 0) return node;
            if (check(this)) return node;
            return null;
        }

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Union(_children.SelectMany(x => x.Flatten()));
        }
    }
}
using System;
using System.Collections.Generic;

namespace WinRTUtil
{
    public class PageResolver : IPageResolver
    {
        private readonly IDictionary<string, Type> _resolvedPages;

        public PageResolver()
        {
            _resolvedPages = new Dictionary<string, Type>();
        }

        public void RegisterPage(string name, Type type)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (type == null)
                throw new ArgumentNullException("name");

            name = name.ToUpper();
            if (_resolvedPages.ContainsKey(name))
                _resolvedPages[name] = type;
            else
                _resolvedPages.Add(name, type);
        }

        public Type Resolve(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            name = name.ToUpper();
            if (_resolvedPages.ContainsKey(name))
                return _resolvedPages[name];
            return null;
        }
    }
}

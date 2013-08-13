using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTUtil
{
    public class NavigationService : INavigationService
    {
        private IPageResolver _pageResolver;
        public Frame CurrentFrame { get { return (Frame)Window.Current.Content; } }

        public NavigationService(IPageResolver pageResolver)
        {
            if (pageResolver == null)
                throw new ArgumentNullException("pageResolver");
            _pageResolver = pageResolver;
        }

        public void Navigate(string pageName)
        {
            Type resolvedPage = _pageResolver.Resolve(pageName);

            if (resolvedPage != null)
                CurrentFrame.Navigate(resolvedPage);
        }

        public void Navigate(string pageName, object dataContext)
        {
            Type resolvedPage = _pageResolver.Resolve(pageName);

            if (resolvedPage != null)
                CurrentFrame.Navigate(resolvedPage, dataContext);
        }

        public void GoBack()
        {
            CurrentFrame.GoBack();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRTUtil
{
    public interface INavigationService
    {
        void Navigate(string pageName);
        void Navigate(string pageName, object dataContext);
        void GoBack();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRTUtil
{
    public interface IPageResolver
    {
        void RegisterPage(string name, Type type);
        Type Resolve(string name);
    }
}

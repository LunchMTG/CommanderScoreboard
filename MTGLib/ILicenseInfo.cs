using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLib
{
    public interface ILicenseInfo
    {
        bool IsTrial { get; }
    }
}

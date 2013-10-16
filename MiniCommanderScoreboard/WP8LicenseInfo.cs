using MTGLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCommanderScoreboard
{
    public class WP8LicenseInfo : ILicenseInfo
    {
        public bool IsTrial
        {
            get { return new Microsoft.Phone.Marketplace.LicenseInformation().IsTrial(); }
        }
    }
}

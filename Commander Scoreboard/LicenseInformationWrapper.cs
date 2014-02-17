using MTGLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Store;

namespace Commander_Scoreboard
{
    public class LicenseInformationWrapper : ILicenseInfo
    {
        public bool IsTrial
        {
            get { return CurrentApp.LicenseInformation.IsTrial; }
        }
    }
}

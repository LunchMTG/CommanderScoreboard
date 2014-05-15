using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Store;
using CommanderScoreboardUniversal.ViewModels;

namespace CommanderScoreboardUniversal
{
    public class LicenseInformationWrapper : ILicenseInfo
    {
        public bool IsTrial
        {
            get { return CurrentApp.LicenseInformation.IsTrial; }
        }
    }
}

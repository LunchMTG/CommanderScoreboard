using MTGLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCommanderScoreboard
{
    public class MTGLibWp8Module : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IPlayerNamesStore>().To<WP8NamesPersistor>();
            Bind<ILicenseInfo>().To<WP8LicenseInfo>();
        }

    }
}

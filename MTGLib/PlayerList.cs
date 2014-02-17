using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MVVM;

namespace MTGLib
{
    public class PlayerList : INotifyPropertyChanged
    {
        public PlayerList(IPlayerNamesStore namesPersister, ILicenseInfo licenseInfo)
        {
            this.licenseInfo = licenseInfo;
            this.namesPersister = namesPersister;
            AvailablePlayers = new ObservableCollection<string>(namesPersister.Load());
            Players = new ObservableCollection<string>();
        }

        public RelayCommand AddPlayer { get { return new RelayCommand(() => { AvailablePlayers.Add(NewPlayerName); Save(); }); } }

        public ObservableCollection<string> AvailablePlayers { get; set; }

        public ObservableCollection<string> Players { get; set; }

        public bool CanStartGame { get { return Players.Count > 1; } }

        public void Save()
        {
            namesPersister.Save(AvailablePlayers.ToArray());
        }

        IPlayerNamesStore namesPersister { get; set; }

        public bool IsCommanderGame { get; set; }
        public bool ShowPoisonControls { get; set; }

        private bool _areGuestsEnabled;

        public bool GuestsEnabled
        {
            get { return _areGuestsEnabled; }
            set { _areGuestsEnabled = value; GuestCount = (int)Math.Max(1, _guestCount); NotifyChanged("GuestCount", "GuestCountText"); }
        }

        private int _guestCount;

        public int GuestCount
        {
            get { return _areGuestsEnabled ? _guestCount : 0; }
            set { _guestCount = value; NotifyChanged("GuestCount", "GuestCountText"); }
        }

        public string GuestCountText { get { return (GuestCount == 0 ? "No " : GuestCount + " ") + "Guest" + (GuestCount != 1 ? "s" : ""); } }

        public void NotifyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
                foreach (var propertyName in propertyNames)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsTrial { get { return licenseInfo.IsTrial; } }

        public event PropertyChangedEventHandler PropertyChanged;
        private ILicenseInfo licenseInfo;

        public string NewPlayerName { get; set; }
    }
}

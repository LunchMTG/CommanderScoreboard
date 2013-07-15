﻿using MyToolkit.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander_Scoreboard
{
    public class StartViewModel
    {
        public StartViewModel()
        {
            PlayerListCache.Load();
        }

        public bool IsCommanderGame { get; set; }
        public ObservableCollection<string> Players { get { return PlayerListCache.Players; } }
        public string NewPlayerName { get; set; }
        public RelayCommand MakeNewPlayer { get { return new RelayCommand(() => { PlayerListCache.Players.Add(NewPlayerName); PlayerListCache.Save(); }); } }

    }
}

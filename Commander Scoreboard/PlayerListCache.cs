﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander_Scoreboard
{
    public static class PlayerListCache
    {
        public static ObservableCollection<string> Players { get; set; }
        public static void Save()
        {
            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;


            if (Players == null) return;

            settings.Values["playerlist"] = string.Join(",", Players);

            //await Windows.Storage.FileIO.WriteTextAsync(createdFile, writeThis);
        }
        public static void Load()
        {
            try
            {
                Players = new ObservableCollection<string>(((string)Windows.Storage.ApplicationData.Current.RoamingSettings.Values["playerlist"]).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
            catch (NullReferenceException)
            {
                Players = new ObservableCollection<string>();
                Save();
            }
        }
    }
}
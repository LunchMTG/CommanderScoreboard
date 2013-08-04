﻿using MyToolkit.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MTGLib
{
    public class Game : INotifyPropertyChanged
    {
        public Game()
        {
            Players = new ObservableCollection<Player>();
            //{
            //    new Player{ Name = "Owen"},
            //    new Player{ Name = "Carlos"},
            //    new Player{ Name = "Judah"},
            //    new Player{ Name = "Ross"},
            //    new Player{ Name = "Scumbag Merek"}
            //};

        }

        public ObservableCollection<Player> Players { get; set; }
        private Player _currentPlayer;
        public Player CurrentPlayer { get { return _currentPlayer; } set { _currentPlayer = value; NotifyChanged("CurrentPlayer", "ShowCommands"); } }

        private void NotifyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
                foreach (var property in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
                }
        }
        public Player CommanderDamageSource { get; set; }

        public bool IsCommanderGame { get; set; }
        public bool ShowCommands { get { return CurrentPlayer != null; } }
        public bool ShowCommanderControls { get { return IsCommanderGame; } }

        private bool _poisonEnabled;

        public bool ShowPoisonControls
        {
            get { return _poisonEnabled; }
            set { _poisonEnabled = value; NotifyChanged("ShowPoisonControls"); }
        }

        public RelayCommand Extort
        {
            get
            {
                return new RelayCommand(() =>
                {
                    foreach (var player in Players)
                    {
                        if (player == CurrentPlayer)
                        {
                            player.Life += Players.Count - 1;
                            player.Refresh();
                        }
                        else
                        {
                            player.Life--;
                            player.Refresh();
                        }
                    }
                });
            }
        }
        public RelayCommand Plus1Life { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.Life++; CurrentPlayer.Refresh(); }); } }
        public RelayCommand Plus5Life { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.Life += 5; CurrentPlayer.Refresh(); }); } }
        public RelayCommand Minus1Life { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.Life--; CurrentPlayer.Refresh(); }); } }
        public RelayCommand Minus5Life { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.Life -= 5; CurrentPlayer.Refresh(); }); } }
        public RelayCommand RemovePoison { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.Poison--; CurrentPlayer.Refresh(); }); } }
        public RelayCommand AddPoison { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.Poison++; CurrentPlayer.Refresh(); }); } }
        public RelayCommand AddCost { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.CommanderAdditionalCost += 2; CurrentPlayer.Refresh(); }); } }
        public RelayCommand RemoveCost { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.CommanderAdditionalCost -= 2; CurrentPlayer.Refresh(); }); } }
        public RelayCommand SendCommanderDamage
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (CurrentPlayer == null || CommanderDamageSource == null) return;

                    if (!CurrentPlayer.CommanderDamage.Any(item => item.DamageSource == CommanderDamageSource))
                        CurrentPlayer.CommanderDamage.Add(new CommanderDamageItem { DamageSource = CommanderDamageSource, Amount = 0 });

                    CurrentPlayer.CommanderDamage.First(item => item.DamageSource == CommanderDamageSource).Amount++;
                    CurrentPlayer.Life--;
                    CurrentPlayer.Refresh();
                });
            }
        }
        public RelayCommand UndoCommanderDamage
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (CurrentPlayer == null || CommanderDamageSource == null) return;

                    if (!CurrentPlayer.CommanderDamage.Any(item => item.DamageSource == CommanderDamageSource))
                        CurrentPlayer.CommanderDamage.Add(new CommanderDamageItem { DamageSource = CommanderDamageSource, Amount = 0 });

                    var damageCounter = CurrentPlayer.CommanderDamage.First(item => item.DamageSource == CommanderDamageSource);

                    CurrentPlayer.Life++;
                    damageCounter.Amount--;
                    if (damageCounter.Amount == 0)
                        CurrentPlayer.CommanderDamage.Remove(damageCounter);

                    CurrentPlayer.Refresh();
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

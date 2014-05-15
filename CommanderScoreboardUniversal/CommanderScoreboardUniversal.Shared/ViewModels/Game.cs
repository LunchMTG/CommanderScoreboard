using MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace CommanderScoreboardUniversal.ViewModels
{
    public class Game : INotifyPropertyChanged
    {
        public Game()
        {
            Players = new ObservableCollection<Player>();
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

        private Player _commanderSource;
        public Player CommanderDamageSource { get { return _commanderSource ?? Players[0]; } set { _commanderSource = value; } }

        public bool IsCommanderGame { get; set; }
        public bool ShowCommands { get { return CurrentPlayer != null; } }
        public bool ShowCommanderControls { get { return IsCommanderGame; } }

        private bool _poisonEnabled;

        public bool ShowPoisonControls
        {
            get { return _poisonEnabled; }
            set { _poisonEnabled = value; NotifyChanged("ShowPoisonControls"); }
        }

        public RelayCommand TogglePoisonControls { get { return new RelayCommand(() => ShowPoisonControls = !ShowPoisonControls); } }

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
        public RelayCommand Plus1Life { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.LifeDelta++; CurrentPlayer.Refresh(); }); } }
        public RelayCommand Plus5Life { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.LifeDelta += 5; CurrentPlayer.Refresh(); }); } }
        public RelayCommand Minus1Life { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.LifeDelta--; CurrentPlayer.Refresh(); }); } }
        public RelayCommand Minus5Life { get { return new RelayCommand(() => { if (CurrentPlayer == null) return; CurrentPlayer.LifeDelta -= 5; CurrentPlayer.Refresh(); }); } }
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

        public RelayCommand ResetGame
        {
            get
            {
                return new RelayCommand(() =>
                    {
                        foreach (var player in Players)
                        {
                            player.CommanderDamage.Clear();
                            player.Poison = 0;
                            player.CommanderAdditionalCost = 0;
                            player.Life = player.IsCommanderGame ? 40 : 20;
                            player.Refresh();
                        }
                    });
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}

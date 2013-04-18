﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander_Scoreboard
{
    public class CommanderDamageItem : INotifyPropertyChanged
    {
        private Player _source;

        public Player DamageSource
        {
            get { return _source; }
            set { _source = value; Notify(); }
        }

        private int _amount;

        public int Amount
        {
            get { return _amount; }
            set { _amount = value; Notify(); }
        }


        private void Notify()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(""));
        }

        //public Player DamageSource { get; set; }
        //public int Amount { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CommanderScoreboardUniversal.ViewModels
{
    [DataContract(IsReference=true)]
    public class CommanderDamageItem : INotifyPropertyChanged
    {
        private Player _source;

        public string DisplayText { get { return string.Format("{0}: {1}", DamageSource.Name, Amount); } }


    [DataMember]
        public Player DamageSource
        {
            get { return _source; }
            set { _source = value; Notify(); }
        }

        private int _amount;

        [DataMember]
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

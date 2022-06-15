using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;
using static Umsetzung_III.Actions;

namespace Umsetzung_III
{
    public class ViewModel : ViewModelBase
    {
        private readonly Model spielanzeige;
        private readonly TimerStore timerStore;
        private readonly StrafenStore strafenHeim;
        private readonly StrafenStore strafenGast;

        public bool ButtonVisibilityStart => timerStore.ButtonVisibilityStart;
        public bool ButtonVisibilityStop => timerStore.ButtonVisibilityStop;

        public bool ButtonVisibilityHeimStrafe => strafenHeim.ButtonVisibilityStrafe;
        public bool ButtonVisibilityHeimReset => strafenHeim.ButtonVisibilityReset;
        public bool ButtonVisibilityGastStrafe => strafenGast.ButtonVisibilityStrafe;
        public bool ButtonVisibilityGastReset => strafenGast.ButtonVisibilityReset;




        public string Spielzeit
            { get { return timerStore.spielzeit; }
        }

        public string HeimTeamStrafe
        {
            get { return strafenHeim.strafzeit; }
        }
        public string GastTeamStrafe
        {
            get { return strafenGast.strafzeit; }
        }

        public int Halbzeit
        {
            get { return spielanzeige.Halbzeit; }
            set
            {
                if (spielanzeige.Halbzeit != value)
                {
                    spielanzeige.Halbzeit = value;
                    OnPropertyChanged("Halbzeit");


                }
            }
        }

        public string HeimTeamName
        {
            get { return spielanzeige.HeimTeamName; }
            set
            {
                if (spielanzeige.HeimTeamName != value)
                {
                    spielanzeige.HeimTeamName = value;
                    OnPropertyChanged("HeimTeamName");
                }
            }
        }

        public string GastTeamName
        {
            get { return spielanzeige.GastTeamName; }
            set
            {
                if (spielanzeige.GastTeamName != value)
                {
                    spielanzeige.GastTeamName = value;
                    OnPropertyChanged("GastTeamName");
                }
            }
        }

        public int GastTeamScore
        {
            get { return spielanzeige.GastTeamScore; }
            set
            {
                if (spielanzeige.GastTeamScore != value)
                {
                    spielanzeige.GastTeamScore = value;
                    OnPropertyChanged("GastTeamScore");
                }
            }
        }

        public int HeimTeamScore
        {
            get { return spielanzeige.HeimTeamScore; }
            set
            {
                if (spielanzeige.HeimTeamScore != value)
                {
                    spielanzeige.HeimTeamScore = value;
                    OnPropertyChanged("HeimTeamScore");
                }
            }
        }
        public ICommand HeimScoreUp { get; }
        public ICommand HeimScoreDown { get; }
        public ICommand GastScoreUp { get; }
        public ICommand GastScoreDown { get; }

        public ICommand StartTime { get; }
        public ICommand StopTime { get; }
        public ICommand ResetTime { get; }
        public ICommand ResetAll { get; }

        public ICommand HeimStrafeZwei { get; }
        public ICommand HeimStrafeFuenf { get; }
        public ICommand HeimStrafeZehn { get; }
        public ICommand HeimStrafeReset { get; }

        public ICommand GastStrafeZwei { get; }
        public ICommand GastStrafeFuenf { get; }
        public ICommand GastStrafeZehn { get; }
        public ICommand GastStrafeReset { get; }




        public ViewModel()
        {
            spielanzeige = new Model();
            this.timerStore = new TimerStore(20, this);
            this.strafenGast = new StrafenStore();
            this.strafenHeim = new StrafenStore();


            this.timerStore.OnSpielzeitChanged += TimerStore_SpielzeitChanged;
            this.timerStore.OnButtonVisibilityChanged += TimerStore_ButtonVisibilityChanged;

            this.strafenHeim.OnStrafzeitChanged += strafenHeim_StrafzeitChanged;
            this.strafenHeim.OnButtonVisibilityChanged += StrafenHeim_ButtonVisibilityChanged;

            this.strafenGast.OnStrafzeitChanged += strafenGast_StrafzeitChanged;
            this.strafenGast.OnButtonVisibilityChanged += StrafenGast_ButtonVisibilityChanged;



            GastScoreUp = new ScoreCommand(this, Team.Gast, StandVeraenderung.Hoch);
            GastScoreDown =new ScoreCommand(this, Team.Gast, StandVeraenderung.Runter);
            HeimScoreUp = new ScoreCommand(this, Team.Heim, StandVeraenderung.Hoch); 
            HeimScoreDown = new ScoreCommand(this, Team.Heim, StandVeraenderung.Runter);

            StartTime = new TimeCommand(this.timerStore, ZeitAktion.Start);
            StopTime = new TimeCommand(this.timerStore, ZeitAktion.Stop);
            ResetTime = new TimeCommand(this.timerStore, ZeitAktion.Reset);

            GastStrafeZwei = new StrafenCommand(Strafe.Zwei, this.strafenGast);
            GastStrafeFuenf = new StrafenCommand(Strafe.Fuenf, this.strafenGast);
            GastStrafeZehn = new StrafenCommand(Strafe.Zehn, this.strafenGast);
            GastStrafeReset = new StrafenCommand(Strafe.Reset, this.strafenGast);

            HeimStrafeZwei = new StrafenCommand(Strafe.Zwei, this.strafenHeim);
            HeimStrafeFuenf = new StrafenCommand(Strafe.Fuenf, this.strafenHeim);
            HeimStrafeZehn = new StrafenCommand(Strafe.Zehn, this.strafenHeim);
            HeimStrafeReset = new StrafenCommand(Strafe.Reset, this.strafenHeim);



        }

        private void TimerStore_SpielzeitChanged()
        {
            OnPropertyChanged("Spielzeit");
        }
        private void TimerStore_ButtonVisibilityChanged()
        {
            OnPropertyChanged("ButtonVisibilityStart");
            OnPropertyChanged("ButtonVisibilityStop");
        }
        private void strafenHeim_StrafzeitChanged()
        {
            OnPropertyChanged("HeimTeamStrafe");
        }

        private void StrafenHeim_ButtonVisibilityChanged()
        {
                OnPropertyChanged("ButtonVisibilityHeimStrafe");
                OnPropertyChanged("ButtonVisibilityHeimReset");
        }

        private void strafenGast_StrafzeitChanged()
        {
            OnPropertyChanged("GastTeamStrafe");
        }
        private void StrafenGast_ButtonVisibilityChanged()
        {
            OnPropertyChanged("ButtonVisibilityGastStrafe");
            OnPropertyChanged("ButtonVisibilityGastReset");
        }


    }
}

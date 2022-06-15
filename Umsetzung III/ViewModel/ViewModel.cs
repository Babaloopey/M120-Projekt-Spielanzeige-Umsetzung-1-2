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
        // MemberVariablen
        private readonly Model spielanzeige;
        private readonly TimerStore timerStore;
        private readonly StrafenStore strafenHeim;
        private readonly StrafenStore strafenGast;

        // Properties, die von der View abgefragt werden, um Buttons zu verstecken/ anzuzeigen
        public bool ButtonVisibilityStart => timerStore.ButtonVisibilityStart;
        public bool ButtonVisibilityStop => !timerStore.ButtonVisibilityStart;

        public bool ButtonVisibilityHeimStrafe => strafenHeim.ButtonVisibilityStrafe;
        public bool ButtonVisibilityHeimReset => !strafenHeim.ButtonVisibilityStrafe;
        public bool ButtonVisibilityGastStrafe => strafenGast.ButtonVisibilityStrafe;
        public bool ButtonVisibilityGastReset => !strafenGast.ButtonVisibilityStrafe;

        // Properties, die von der View abgefragt werden, um Informationen darzustellen
        public string Spielzeit
            { get { return timerStore.spielzeit; }
        }
        public string HeimTeamStrafe
        {
            get { return strafenHeim.Strafzeit; }
        }
        public string GastTeamStrafe
        {
            get { return strafenGast.Strafzeit; }
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

        // Definition der Buttons der Views
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

        // Zuruecksetzen des gesamten ViewModels auf den Anfangszustand
        public void ResetViewModel()
        {
            this.spielanzeige.ResetModel();
            this.timerStore.Reset();
            this.strafenGast.Reset();
            this.strafenHeim.Reset();
            OnPropertyChanged("HeimTeamScore");
            OnPropertyChanged("GastTeamScore");
            OnPropertyChanged("HeimTeamName");
            OnPropertyChanged("GastTeamName");
        }

        public ViewModel()
        {
            // Initialisierung des Models und der Stores
            spielanzeige = new Model();
            this.timerStore = new TimerStore(20, this);
            this.strafenGast = new StrafenStore();
            this.strafenHeim = new StrafenStore();

            // EventBinding
            this.timerStore.OnSpielzeitChanged += TimerStore_SpielzeitChanged;
            this.timerStore.OnButtonVisibilityChanged += TimerStore_ButtonVisibilityChanged;
            this.timerStore.OnZeitGestoppt += TimerStore_ZeitGestoppt;
            this.timerStore.OnZeitGestartet += TimerStore_ZeitGestartet;

            this.strafenHeim.OnStrafzeitChanged += strafenHeim_StrafzeitChanged;
            this.strafenHeim.OnButtonVisibilityChanged += StrafenHeim_ButtonVisibilityChanged;

            this.strafenGast.OnStrafzeitChanged += strafenGast_StrafzeitChanged;
            this.strafenGast.OnButtonVisibilityChanged += StrafenGast_ButtonVisibilityChanged;

            // Zuteilung fuer Buttons
            // Buttons fuer die Kontrolle des Punktestandes
            GastScoreUp = new ScoreCommand(this, Team.Gast, StandVeraenderung.Hoch);
            GastScoreDown =new ScoreCommand(this, Team.Gast, StandVeraenderung.Runter);
            HeimScoreUp = new ScoreCommand(this, Team.Heim, StandVeraenderung.Hoch); 
            HeimScoreDown = new ScoreCommand(this, Team.Heim, StandVeraenderung.Runter);

            // Buttons fuer die Kontrolle der Spielzeit
            StartTime = new TimeCommand(this.timerStore, ZeitAktion.Start);
            StopTime = new TimeCommand(this.timerStore, ZeitAktion.Stop);
            ResetTime = new TimeCommand(this.timerStore, ZeitAktion.Reset);

            // Buttons fuer die Kontrolle der Strafen: Gast
            GastStrafeZwei = new StrafenCommand( this.strafenGast, Strafe.Zwei);
            GastStrafeFuenf = new StrafenCommand( this.strafenGast, Strafe.Fuenf);
            GastStrafeZehn = new StrafenCommand(this.strafenGast, Strafe.Zehn);
            GastStrafeReset = new StrafenCommand(this.strafenGast, Strafe.Reset);

            // Buttons fuer die Kontrolle der Strafen: Heim
            HeimStrafeZwei = new StrafenCommand(this.strafenHeim, Strafe.Zwei);
            HeimStrafeFuenf = new StrafenCommand(this.strafenHeim, Strafe.Fuenf);
            HeimStrafeZehn = new StrafenCommand(this.strafenHeim, Strafe.Zehn);
            HeimStrafeReset = new StrafenCommand(this.strafenHeim, Strafe.Reset);

            // Button um das ViewModel zurueckzusetzen
            ResetAll = new ResetAllCommand(this);
        }

        // Funktionen, die an die Events der Stores gebunden sind: Im Konstruktor verlinkt
        private void TimerStore_SpielzeitChanged()
        {
            OnPropertyChanged("Spielzeit");
        }
        private void TimerStore_ButtonVisibilityChanged()
        {
            OnPropertyChanged("ButtonVisibilityStart");
            OnPropertyChanged("ButtonVisibilityStop");
        }
        private void TimerStore_ZeitGestoppt()
        {
            strafenHeim.Stop();
            strafenGast.Stop();
        }
        private void TimerStore_ZeitGestartet()
        {
            strafenHeim.Resume();
            strafenGast.Resume();

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

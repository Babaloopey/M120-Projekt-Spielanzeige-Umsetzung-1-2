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
    public class SpielanzeigeViewModel : ViewModelBase
    {
        // MemberVariablen
        private readonly SpielanzeigeModel _spielanzeige;
        private readonly TimerStore _timerStore;
        private readonly StrafenStore _strafenHeim;
        private readonly StrafenStore _strafenGast;

        // Properties, die von der View abgefragt werden, um Buttons zu verstecken/ anzuzeigen
        public bool ButtonVisibilityStart => _timerStore.ButtonVisibilityStart;
        public bool ButtonVisibilityStop => !_timerStore.ButtonVisibilityStart;

        public bool ButtonVisibilityHeimStrafe => _strafenHeim.ButtonVisibilityStrafe;
        public bool ButtonVisibilityHeimReset => !_strafenHeim.ButtonVisibilityStrafe;
        public bool ButtonVisibilityGastStrafe => _strafenGast.ButtonVisibilityStrafe;
        public bool ButtonVisibilityGastReset => !_strafenGast.ButtonVisibilityStrafe;

        // Properties, die von der View abgefragt werden, um Informationen darzustellen
        public string Spielzeit
            { get { return _timerStore.Spielzeit; }
        }
        public string HeimTeamStrafe
        {
            get { return _strafenHeim.Strafzeit; }
        }
        public string GastTeamStrafe
        {
            get { return _strafenGast.Strafzeit; }
        }
        public int Halbzeit
        {
            get { return _spielanzeige.Halbzeit; }
            set
            {
                if (_spielanzeige.Halbzeit != value)
                {
                    _spielanzeige.Halbzeit = value;
                    OnPropertyChanged("Halbzeit");
                }
            }
        }
        public string HeimTeamName
        {
            get { return _spielanzeige.HeimTeamName; }
            set
            {
                if (_spielanzeige.HeimTeamName != value)
                {
                    _spielanzeige.HeimTeamName = value;
                    OnPropertyChanged("HeimTeamName");
                }
            }
        }
        public string GastTeamName
        {
            get { return _spielanzeige.GastTeamName; }
            set
            {
                if (_spielanzeige.GastTeamName != value)
                {
                    _spielanzeige.GastTeamName = value;
                    OnPropertyChanged("GastTeamName");
                }
            }
        }
        public int GastTeamScore
        {
            get { return _spielanzeige.GastTeamScore; }
            set
            {
                if (_spielanzeige.GastTeamScore != value)
                {
                    _spielanzeige.GastTeamScore = value;
                    OnPropertyChanged("GastTeamScore");
                }
            }
        }
        public int HeimTeamScore
        {
            get { return _spielanzeige.HeimTeamScore; }
            set
            {
                if (_spielanzeige.HeimTeamScore != value)
                {
                    _spielanzeige.HeimTeamScore = value;
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
        public ICommand SpaceButton { get; }
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
            _spielanzeige.ResetModel();
            _timerStore.Reset();
            _strafenGast.Reset();
            _strafenHeim.Reset();
            OnPropertyChanged("HeimTeamScore");
            OnPropertyChanged("GastTeamScore");
            OnPropertyChanged("HeimTeamName");
            OnPropertyChanged("GastTeamName");
        }

        public SpielanzeigeViewModel()
        {
            // Initialisierung des Models und der Stores
            _spielanzeige = new SpielanzeigeModel();
            _timerStore = new TimerStore(20, this);
            _strafenGast = new StrafenStore();
            _strafenHeim = new StrafenStore();

            // EventBinding
            _timerStore.OnSpielzeitChanged += TimerStore_SpielzeitChanged;
            _timerStore.OnButtonVisibilityChanged += TimerStore_ButtonVisibilityChanged;
            _timerStore.OnZeitGestoppt += TimerStore_ZeitGestoppt;
            _timerStore.OnZeitGestartet += TimerStore_ZeitGestartet;

            _strafenHeim.OnStrafzeitChanged += StrafenHeim_StrafzeitChanged;
            _strafenHeim.OnButtonVisibilityChanged += StrafenHeim_ButtonVisibilityChanged;

            _strafenGast.OnStrafzeitChanged += StrafenGast_StrafzeitChanged;
            _strafenGast.OnButtonVisibilityChanged += StrafenGast_ButtonVisibilityChanged;

            // Zuteilung fuer Buttons
            // Buttons fuer die Kontrolle des Punktestandes
            GastScoreUp = new ScoreCommand(this, Team.Gast, StandVeraenderung.Hoch);
            GastScoreDown =new ScoreCommand(this, Team.Gast, StandVeraenderung.Runter);
            HeimScoreUp = new ScoreCommand(this, Team.Heim, StandVeraenderung.Hoch); 
            HeimScoreDown = new ScoreCommand(this, Team.Heim, StandVeraenderung.Runter);

            // Buttons fuer die Kontrolle der Spielzeit
            StartTime = new TimeCommand(_timerStore, ZeitAktion.Start);
            StopTime = new TimeCommand(_timerStore, ZeitAktion.Stop);
            ResetTime = new TimeCommand(_timerStore, ZeitAktion.Reset);
            SpaceButton = new TimeCommand(_timerStore, ZeitAktion.Space);


            // Buttons fuer die Kontrolle der Strafen: Gast
            GastStrafeZwei = new StrafenCommand( _strafenGast, Strafe.Zwei);
            GastStrafeFuenf = new StrafenCommand( _strafenGast, Strafe.Fuenf);
            GastStrafeZehn = new StrafenCommand(_strafenGast, Strafe.Zehn);
            GastStrafeReset = new StrafenCommand(_strafenGast, Strafe.Reset);

            // Buttons fuer die Kontrolle der Strafen: Heim
            HeimStrafeZwei = new StrafenCommand(_strafenHeim, Strafe.Zwei);
            HeimStrafeFuenf = new StrafenCommand(_strafenHeim, Strafe.Fuenf);
            HeimStrafeZehn = new StrafenCommand(_strafenHeim, Strafe.Zehn);
            HeimStrafeReset = new StrafenCommand(_strafenHeim, Strafe.Reset);

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
            _strafenHeim.Stop();
            _strafenGast.Stop();
        }
        private void TimerStore_ZeitGestartet()
        {
            _strafenHeim.Resume();
            _strafenGast.Resume();

        }

        private void StrafenHeim_StrafzeitChanged()
        {
            OnPropertyChanged("HeimTeamStrafe");
        }
        private void StrafenHeim_ButtonVisibilityChanged()
        {
                OnPropertyChanged("ButtonVisibilityHeimStrafe");
                OnPropertyChanged("ButtonVisibilityHeimReset");
        }

        private void StrafenGast_StrafzeitChanged()
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

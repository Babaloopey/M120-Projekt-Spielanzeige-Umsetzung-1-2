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
using Umsetzung_III.Stores;
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
        private readonly LogoStore _logoStore;

        // Properties, die von der View abgefragt werden, um Buttons zu verstecken/ anzuzeigen
        public bool ButtonVisibilityStart => _timerStore.ButtonVisibilityStart;
        public bool ButtonVisibilityStop => !_timerStore.ButtonVisibilityStart;
        public bool EffektiveSpielzeitVisibility => _timerStore.EffektiveSpielzeitVisibility;

        public bool ButtonVisibilityHeimStrafe => _strafenHeim.ButtonVisibilityStrafe;
        public bool ButtonVisibilityHeimReset => !_strafenHeim.ButtonVisibilityStrafe;
        public bool ButtonVisibilityGastStrafe => _strafenGast.ButtonVisibilityStrafe;
        public bool ButtonVisibilityGastReset => !_strafenGast.ButtonVisibilityStrafe;
        public bool LogoVisibility => _logoStore.LogoVisibility;

        // Properties, die von der View abgefragt werden, um Informationen darzustellen
        public string Spielzeit
            { get { return _timerStore.Spielzeit; }
        }
        public int SpielMinute
        { get { return _timerStore.SpielMinute; } }
        public int SpielSekunde
        { get { return _timerStore.SpielSekunde; } }
        public string HeimTeamStrafe
        {
            get { return _strafenHeim.Strafzeit; }
        }
        public string GastTeamStrafe
        {
            get { return _strafenGast.Strafzeit; }
        }
        public bool HeimTeamStrafeRunning
        {
            get { return _strafenHeim.StrafeIsRunning; }
        }
        public bool GastTeamStrafeRunning
        {
            get { return _strafenGast.StrafeIsRunning; }
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
        public ICommand TimeMinusOne { get; }
        public ICommand TimePlusOne { get; }

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
            Console.Beep(250, 1000);
            // Initialisierung des Models und der Stores
            _spielanzeige = new SpielanzeigeModel();
            _timerStore = new TimerStore(20, this);
            _strafenGast = new StrafenStore(this);
            _strafenHeim = new StrafenStore(this);
            _logoStore = new LogoStore(this);

            // EventBinding
            _timerStore.OnSpielzeitChanged += TimerStore_SpielzeitChanged;
            _timerStore.OnButtonVisibilityChanged += TimerStore_ButtonVisibilityChanged;
            _timerStore.EffektiveSpielzeitVisibilityChanged += TimerStore_EffektiveSpielzeitVisibilityChanged;
            _timerStore.OnTimerElapsed += TimerStore_TimerElapsed;

            _strafenHeim.OnStrafzeitChanged += StrafenHeim_StrafzeitChanged;
            _strafenHeim.OnButtonVisibilityChanged += StrafenHeim_ButtonVisibilityChanged;

            _strafenGast.OnStrafzeitChanged += StrafenGast_StrafzeitChanged;
            _strafenGast.OnButtonVisibilityChanged += StrafenGast_ButtonVisibilityChanged;

            _logoStore.OnLogoVisibilityChanged += LogoStore_LogoVisibilityChanged; ;

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
            TimeMinusOne = new TimeCommand(_timerStore, ZeitAktion.MinusOne);
            TimePlusOne = new TimeCommand(_timerStore, ZeitAktion.PlusOne);



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
        private void TimerStore_EffektiveSpielzeitVisibilityChanged()
        {
            OnPropertyChanged("EffektiveSpielzeitVisibility");
        }

        private void TimerStore_TimerElapsed()
        {
            _strafenGast.CheckIfStrafeStillActive();
            _strafenHeim.CheckIfStrafeStillActive();
        }

        private void StrafenHeim_StrafzeitChanged()
        {
            OnPropertyChanged("HeimTeamStrafe");
        }
        private void StrafenHeim_ButtonVisibilityChanged()
        {
                OnPropertyChanged("ButtonVisibilityHeimStrafe");
                OnPropertyChanged("ButtonVisibilityHeimReset");
                _logoStore.CheckIfLogoMustBeVisible();
        }

        private void StrafenGast_StrafzeitChanged()
        {
            OnPropertyChanged("GastTeamStrafe");
        }
        private void StrafenGast_ButtonVisibilityChanged()
        {
            OnPropertyChanged("ButtonVisibilityGastStrafe");
            OnPropertyChanged("ButtonVisibilityGastReset");
            _logoStore.CheckIfLogoMustBeVisible();

        }
        private void LogoStore_LogoVisibilityChanged()
        {
            OnPropertyChanged("LogoVisibility");
        }
    }
}

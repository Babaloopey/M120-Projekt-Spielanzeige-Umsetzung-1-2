using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Media;

namespace Umsetzung_III
{
    public class TimerStore
    {
        private readonly SpielanzeigeViewModel _viewModel;
        private readonly Timer _timer;
        private int _sekunde;
        private int _minute;
        private readonly int _duration;

        public bool ButtonVisibilityStart;
        public string Spielzeit => _minute.ToString("00") + ":" + _sekunde.ToString("00");

        public event Action OnSpielzeitChanged;
        public event Action OnButtonVisibilityChanged;
        public event Action OnZeitGestartet;
        public event Action OnZeitGestoppt;
        public TimerStore(int duration, SpielanzeigeViewModel viewModel)
        {
            _viewModel = viewModel;
            _duration = 2;

            _timer = new Timer(1000);
            _timer.Elapsed += Timer_Elapsed;

            _sekunde = 0;
            _minute = _duration;

            ButtonVisibilityStart = true;
            ButtonVisibilityChanged();
        }
        public void Start()
        {
            _timer.Start();
            ZeitGestartet();
            SpielzeitChanged();

            ButtonVisibilityStart = false;
            ButtonVisibilityChanged();
        }
        public void Stop()
        {
            _timer.Stop();
            ZeitGestoppt();

            ButtonVisibilityStart = true;
            ButtonVisibilityChanged();
        }
        public void Reset()
        {
            _timer.Stop();
            _minute = _duration;
            _sekunde = 0;
            SpielzeitChanged();

            ButtonVisibilityStart = true;
            ButtonVisibilityChanged();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(_sekunde == 0)
            {
                _minute--;
                _sekunde = 59;
            }
            else
            {
                _sekunde--;
            }

            if(_minute == 0 && _sekunde == 0)
            {
                SpielzeitAbgelaufen();
            }

            SpielzeitChanged();
        }

        private void SpielzeitAbgelaufen()
        {
            // Soundstore etwas hackish bedient. Bei Instanzierung im Konstruktor und Execution hier meldet soundStore.Play, dass sich
            // das abzuspielende Objekt in einem anderen Thread befindet
            new SoundStore().Play();
            Stop();
            Timer wartezeit = new Timer(5000);
            wartezeit.Start();
            wartezeit.Elapsed += (sender, args) =>
            {
                
                Reset();
                _viewModel.Halbzeit += 1;
                wartezeit.Dispose();
            };
        }

        private void SpielzeitChanged()
        {
            OnSpielzeitChanged?.Invoke();
        }
        private void ButtonVisibilityChanged()
        {
            OnButtonVisibilityChanged?.Invoke();
        }
        private void ZeitGestoppt()
        {
            OnZeitGestoppt?.Invoke();
        }
        private void ZeitGestartet()
        {
            OnZeitGestartet?.Invoke();
        }
    }
}

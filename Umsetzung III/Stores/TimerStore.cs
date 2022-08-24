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
        private int _timerIterations = 0;

        public bool ButtonVisibilityStart;
        public bool EffektiveSpielzeitVisibility;
        public string Spielzeit => _minute.ToString("00") + ":" + _sekunde.ToString("00");

        public event Action OnSpielzeitChanged;
        public event Action OnButtonVisibilityChanged;
        public event Action OnZeitGestartet;
        public event Action OnZeitGestoppt;
        public event Action EffektiveSpielzeitVisibilityChanged;
        public TimerStore(int duration, SpielanzeigeViewModel viewModel)
        {
            _viewModel = viewModel;
            _duration = duration;

            _timer = new Timer(100);
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
            SpielzeitChanged();


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
        public void MinutePlusOne()
        {
            _minute++;
            SpielzeitChanged();
            CheckEffektiveSpielzeit();
        }
        public void MinuteMinusOne()
        {
            if (_minute > 0)
            {
                _minute--;
                SpielzeitChanged();
                CheckEffektiveSpielzeit();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timerIterations++;

            if(_timerIterations >= 10)
            {
               CountOneSecond();
            }
        }

        private void CountOneSecond()
        {
            _timerIterations = 0;

            if (_sekunde == 0)
            {
                MinuteMinusOne();
                _sekunde = 59;
            }
            else
            {
                _sekunde--;
            }

            if (_minute == 0 && _sekunde == 0)
            {
                SpielzeitAbgelaufen();
            }

            SpielzeitChanged();
        }

        private void CheckEffektiveSpielzeit()
        {
            if(_minute < 3 && _viewModel.Halbzeit==2)
            {
                EffektiveSpielzeitVisibility = true;
            }
            else
            {
                EffektiveSpielzeitVisibility = false;
            }
            EffektiveSpielzeitChanged();
        }

        private void SpielzeitAbgelaufen()
        {

            Stop();

            Console.Beep(250,2000);

            Timer wartezeit = new Timer(3000);
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
        private void EffektiveSpielzeitChanged()
        {
            EffektiveSpielzeitVisibilityChanged?.Invoke();
        }
    }
}

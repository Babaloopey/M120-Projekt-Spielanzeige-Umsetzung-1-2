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
        private ViewModel viewModel;
        private readonly Timer timer;
        private int sekunde;
        private int minute;

        private int duration;

        public bool ButtonVisibilityStart;


        public string spielzeit => minute.ToString("00") + ":" + sekunde.ToString("00");

        public event Action OnSpielzeitChanged;
        public event Action OnButtonVisibilityChanged;
        public event Action OnZeitGestartet;
        public event Action OnZeitGestoppt;
        public TimerStore(int duration, ViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.duration = duration;

            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;

            sekunde = 0;
            minute = this.duration;

            ButtonVisibilityStart = true;
            ButtonVisibilityChanged();
        }
        public void Start()
        {
            timer.Start();
            ZeitGestartet();
            SpielzeitChanged();

            ButtonVisibilityStart = false;
            ButtonVisibilityChanged();
        }
        public void Stop()
        {
            timer.Stop();
            ZeitGestoppt();

            ButtonVisibilityStart = true;
            ButtonVisibilityChanged();
        }
        public void Reset()
        {
            timer.Stop();
            this.minute = this.duration;
            this.sekunde = 0;
            SpielzeitChanged();

            ButtonVisibilityStart = true;
            ButtonVisibilityChanged();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(sekunde == 0)
            {
                minute--;
                sekunde = 59;
            }
            else
            {
                sekunde--;
            }

            if(minute == 0 && sekunde == 0)
            {
                SpielzeitAbgelaufen();
            }

            SpielzeitChanged();
        }

        private void SpielzeitAbgelaufen()
        {
            this.Stop();

            Timer wartezeit = new Timer(5000);
            wartezeit.Start();
            wartezeit.Elapsed += (sender, args) =>
            {
                this.Reset();
                viewModel.Halbzeit += 1;
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

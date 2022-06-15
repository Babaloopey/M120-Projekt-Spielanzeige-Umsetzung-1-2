using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static Umsetzung_III.Actions;

namespace Umsetzung_III
{
    public class StrafenStore
    {
        private readonly Timer timer;
        private int sekunde;
        private int minute;

        public string strafzeit => minute.ToString("00") + ":" + sekunde.ToString("00");

        public bool ButtonVisibilityStrafe;

        public event Action OnStrafzeitChanged;
        public event Action OnButtonVisibilityChanged;

        public StrafenStore()
        {

            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;

            sekunde = 0;
            minute = 0;

            ButtonVisibilityStrafe = true;
        }
        public void Start(Strafe strafe)
        {
            switch (strafe)
            {
                case Strafe.Zwei:
                    minute = 2;
                    break;
                case Strafe.Fuenf:
                    minute = 5;
                    break;
                case Strafe.Zehn:
                    minute = 10;
                    break;
            }
            timer.Start();
            StrafzeitChanged();

            ButtonVisibilityStrafe = false;
            ButtonVisibilityChanged();
        }

        public void Resume()
        {
            if(minute >= 0 && sekunde > 0)
            {
                timer.Start();
                StrafzeitChanged();
            }
            
        }
        public void Stop()
        {
            timer.Stop();
            StrafzeitChanged();

        }
        public void Reset()
        {
            timer.Stop();
            this.minute = 0;
            this.sekunde = 0;
            StrafzeitChanged();

            ButtonVisibilityStrafe = true;
            ButtonVisibilityChanged();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (sekunde == 0)
            {
                minute--;
                sekunde = 59;
            }
            else
            {
                sekunde--;
            }

            if (minute == 0 && sekunde == 0)
            {
                StrafzeitAbgelaufen();
            }

            StrafzeitChanged();
        }

        private void StrafzeitAbgelaufen()
        {
            timer.Stop();

            Timer wartezeit = new Timer(5000);
            wartezeit.Start();
            wartezeit.Elapsed += (sender, args) =>
            {
                wartezeit.Stop();
                this.Reset();
            };
        }

        private void StrafzeitChanged()
        {
            OnStrafzeitChanged?.Invoke();
        }

        private void ButtonVisibilityChanged()
        {
            OnButtonVisibilityChanged?.Invoke();
        }
    }
}

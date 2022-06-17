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
        private readonly Timer _timer;
        private int _sekunde;
        private int _minute;
        public string Strafzeit => _minute.ToString("00") + ":" + _sekunde.ToString("00");
        public bool ButtonVisibilityStrafe;

        public event Action OnStrafzeitChanged;
        public event Action OnButtonVisibilityChanged;

        public StrafenStore()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += Timer_Elapsed;

            _sekunde = 0;
            _minute = 0;

            ButtonVisibilityStrafe = true;
            ButtonVisibilityChanged();

        }
        public void Start(Strafe strafe)
        {
            switch (strafe)
            {
                case Strafe.Zwei:
                    _minute = 2;
                    break;
                case Strafe.Fuenf:
                    _minute = 5;
                    break;
                case Strafe.Zehn:
                    _minute = 10;
                    break;
            }
            StrafzeitChanged();

            _timer.Start();

            ButtonVisibilityStrafe = false;
            ButtonVisibilityChanged();

        }

        public void Resume()
        {
            if(_minute > 0 || _sekunde > 0)
            {
                _timer.Start();
            }
            
        }
        public void Stop()
        {
            _timer.Stop();
        }
        public void Reset()
        {
            _timer.Stop();
            _minute = 0;
            _sekunde = 0;
            StrafzeitChanged();

            ButtonVisibilityStrafe = true;
            ButtonVisibilityChanged();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_sekunde == 0)
            {
                _minute--;
                _sekunde = 59;
            }
            else
            {
                _sekunde--;
            }

            if (_minute == 0 && _sekunde == 0)
            {
                StrafzeitAbgelaufen();
            }

            StrafzeitChanged();
        }

        private void StrafzeitAbgelaufen()
        {
            Stop();
            Timer wartezeit = new Timer(5000);
            wartezeit.Start();
            wartezeit.Elapsed += (sender, args) =>
            {
                Reset();
                wartezeit.Dispose();
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

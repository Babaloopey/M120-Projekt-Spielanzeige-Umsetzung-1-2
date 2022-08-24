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
        private readonly SpielanzeigeViewModel _spielanzeigeViewModel;

        private int _strafMinute;
        private int _strafSekunde;
        public string Strafzeit => _strafMinute.ToString("00") + ":" + _strafSekunde.ToString("00");
        public bool ButtonVisibilityStrafe;
        public bool StrafeIsRunning => !ButtonVisibilityStrafe;

        public event Action OnStrafzeitChanged;
        public event Action OnButtonVisibilityChanged;

        public StrafenStore(SpielanzeigeViewModel spielanzeigeviewModel)
        {
            _spielanzeigeViewModel = spielanzeigeviewModel;

            ButtonVisibilityStrafe = true;
            ButtonVisibilityChanged();

        }
        public void Start(Strafe strafe)
        {
            switch (strafe)
            {
                case Strafe.Zwei:
                    _strafMinute = _spielanzeigeViewModel.SpielMinute - 2;
                    break;
                case Strafe.Fuenf:
                    _strafMinute = _spielanzeigeViewModel.SpielMinute - 5;
                    break;
                case Strafe.Zehn:
                    _strafMinute = _spielanzeigeViewModel.SpielMinute - 10;
                    break;
            }

            if(_strafMinute < 0)
            {
                _strafMinute = 20 + _strafMinute;
            }

            _strafSekunde = _spielanzeigeViewModel.SpielSekunde;

            StrafzeitChanged();

            ButtonVisibilityStrafe = false;
            ButtonVisibilityChanged();

        }

        public void CheckIfStrafeStillActive()
        {
            if(_strafMinute == _spielanzeigeViewModel.SpielMinute && _strafSekunde == _spielanzeigeViewModel.SpielSekunde)
            {
            Timer wartezeit = new Timer(3000);
            wartezeit.Start();
                wartezeit.Elapsed += (sender, args) =>
                {

                    Reset();
                    wartezeit.Dispose();
                };
            }
        }
        public void Reset()
        {
            _strafMinute = 0;
            _strafSekunde = 0;
            StrafzeitChanged();

            ButtonVisibilityStrafe = true;
            ButtonVisibilityChanged();   
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Umsetzung_III.Actions;

namespace Umsetzung_III
{
    // Command, der je Nach Strafe und Store beim jeweiligen Team eine Strafe beginnt
    internal class StrafenCommand : CommandBase
    {
        private readonly Strafe _strafe;
        private readonly StrafenStore _strafenStore;

        public StrafenCommand(StrafenStore strafenStore, Strafe strafe)
        {
            _strafe = strafe;
            _strafenStore = strafenStore;
        }

        public override void Execute(object? parameter)
        {
            switch (_strafe)
            {
                case Strafe.Reset:
                    _strafenStore.Reset();
                    break;
                default:
                    _strafenStore.Start(_strafe);
                    break;
            }
        }
    }
}

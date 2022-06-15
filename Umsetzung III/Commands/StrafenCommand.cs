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
        private readonly Strafe strafe;
        private readonly StrafenStore strafenStore;

        public StrafenCommand(StrafenStore strafenStore, Strafe strafe)
        {
            this.strafe = strafe;
            this.strafenStore = strafenStore;
        }

        public override void Execute(object? parameter)
        {
            switch (strafe)
            {
                case Strafe.Reset:
                    strafenStore.Reset();
                    break;
                default:
                    strafenStore.Start(strafe);
                    break;
            }
        }
    }
}

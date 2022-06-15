using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Umsetzung_III.Actions;

namespace Umsetzung_III
{
    public class StrafenCommand : CommandBase
    {
        Strafe strafe;
        StrafenStore strafenStore;

        public StrafenCommand(Strafe strafe, StrafenStore strafenStore)
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

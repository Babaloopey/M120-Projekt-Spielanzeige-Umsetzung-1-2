using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Umsetzung_III.Actions;

namespace Umsetzung_III
{
    internal class TimeCommand : CommandBase
    {
        private readonly TimerStore timerStore;
        private readonly ZeitAktion zeitAktion;

        public TimeCommand(TimerStore timerStore,ZeitAktion zeitAktion)
        {
            this.timerStore = timerStore;
            this.zeitAktion = zeitAktion;

        }

        public override void Execute(object? parameter)
        {
            switch (zeitAktion)
            {
                case ZeitAktion.Start:
                    timerStore.Start();
                    break;
                case ZeitAktion.Stop:
                    timerStore.Stop();
                    break;
                case ZeitAktion.Reset:
                    timerStore.Reset();
                    break;

            }

        }
    }
}

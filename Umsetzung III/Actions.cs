using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umsetzung_III
{
    public class Actions
    {
        public enum Team { Gast, Heim}
        public enum StandVeraenderung { Hoch, Runter}
        public enum Strafe { Zwei, Fuenf, Zehn, Reset}
        public enum ZeitAktion { Start, Stop, Reset}
    }
}

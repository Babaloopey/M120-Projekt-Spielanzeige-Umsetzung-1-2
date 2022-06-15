using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Umsetzung_III
{
    public class SoundStore
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public SoundStore()
        {
            mediaPlayer.Open(new Uri("C:/Users/vmadmin/OneDrive/01_Standardordner/Schule/2. Sem/Visual Studio Source/M120_226B Projekt UH Spielanzeige/Umsetzung III/Buzzer-Sound.wav"));
        }

        public void Play()
        {
            mediaPlayer.Play();
        }

    }
}

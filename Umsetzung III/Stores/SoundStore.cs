using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Umsetzung_III
{
    public class SoundStore
    {
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        private Uri _audioFileUri;

        public SoundStore()
        {
        }

        public void Play()
        {
            string executableFilePath = Assembly.GetExecutingAssembly().Location;
            string executableDirectoryPath = Path.GetDirectoryName(executableFilePath);
            string audioFilePath = Path.Combine(executableDirectoryPath, "Buzzer-Sound.wav");

            _audioFileUri = new Uri(audioFilePath);
            _mediaPlayer.Open(_audioFileUri);
            _mediaPlayer.Play();
            Trace.WriteLine("Sound");
        }

    }
}

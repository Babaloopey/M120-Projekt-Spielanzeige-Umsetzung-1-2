using System;
using System.Collections.Generic;
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
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private Uri audioFileUri;

        public SoundStore()
        {
            GetPathToSoundFile();
        }
        private void GetPathToSoundFile()
        {
            string executableFilePath = Assembly.GetExecutingAssembly().Location;
            string executableDirectoryPath = Path.GetDirectoryName(executableFilePath);
            string audioFilePath = Path.Combine(executableDirectoryPath, "Buzzer-Souasdfnd.wav");
            audioFileUri = new Uri(audioFilePath);
        }

        public void Play()
        {
            mediaPlayer.Open(audioFileUri);
            mediaPlayer.Play();
        }

    }
}

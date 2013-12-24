using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using VKDesktop.Core.Messages;
using System.IO;
using System.Reflection;
using System.Windows.Resources;
using System.Windows;

namespace VKDesktop.Helpers
{
    public class Notificator
    {
        public static void NewMessage(Message message)
        {
            PlaySound(NotificationSound.Message);
        }
        public static void PlaySound(NotificationSound sound)
        {

            IWavePlayer waveOutDevice;
            AudioFileReader audioFileReader;
            waveOutDevice = new WaveOut();

            audioFileReader = new AudioFileReader("resources/sounds/message.mp3");
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();

        }
        private static WaveStream CreateInputStream(Stream fileName)
        {
            WaveChannel32 inputStream;
            WaveStream mp3Reader = new Mp3FileReader(fileName);
            inputStream = new WaveChannel32(mp3Reader);
            var volumeStream = inputStream;
            return volumeStream;
        }
        public enum NotificationSound
        {
            Message,
            Other
        }
    }
}

﻿using System;
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
using Hardcodet.Wpf.TaskbarNotification;
using VKDesktop.Models;
using System.Windows.Controls.Primitives;

namespace VKDesktop.Helpers
{
    public class Notificator
    {
        public static void NewMessage(Message message)
        {
            PlaySound(NotificationSound.Message);
        }

        public static void ShowMessagePopup(Message message) 
        {

            MessagePopup balloon = new MessagePopup();

            //show balloon and close it after 4 seconds
            Tray.ShowCustomBalloon(balloon, PopupAnimation.Slide, null);
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

        public enum NotificationSound
        {
            Message,
            Other
        }

        public static TaskbarIcon Tray { get; set; }
    }
}

using NAudio.Wave;
using Service.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Infrastructure
{
    public class AudioPlayer : IMediaPlayer
    {
        public bool Disabled { get; set; } = false;

        public AudioPlayer()
        {

        }

        public async Task Play(string fileToPlay)
        {
            if (Disabled) return;

            using (var audioFile = new AudioFileReader(fileToPlay))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    await Task.Delay(1000);
                }
            }
            
        }
    }
}

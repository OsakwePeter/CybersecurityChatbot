using System;
using System.IO;
using System.Media;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Handles playback of the recorded voice greeting on startup.
    /// </summary>
    public static class VoiceGreeting
    {
        private const string WavFileName = "greeting.wav";

        /// <summary>
        /// Plays the WAV greeting file synchronously.
        /// Fails silently if the file is missing or audio is unavailable.
        /// </summary>
        public static void Play()
        {
            string wavPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, WavFileName);

            if (!File.Exists(wavPath))
                return;

            try
            {
                SoundPlayer player = new SoundPlayer(wavPath);
                player.PlaySync();
            }
            catch
            {
                // Audio failure must never crash the chatbot
            }
        }
    }
}
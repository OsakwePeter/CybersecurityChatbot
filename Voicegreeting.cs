// ============================================================
//  VoiceGreeting.cs
//  Responsibility: Plays the WAV voice greeting on startup.
//  Notes        : Uses Reflection so the project compiles and
//                 runs on non-Windows platforms without errors.
//                 If audio fails for any reason the chatbot
//                 continues normally — audio is non-critical.
// ============================================================

using System;
using System.IO;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Handles playback of the recorded voice greeting when the application starts.
    /// Uses Reflection to load System.Media.SoundPlayer at runtime so the project
    /// remains cross-platform — on non-Windows systems the method exits silently.
    /// </summary>
    public static class VoiceGreeting
    {
        // Name of the WAV file that must exist in the application output directory.
        // To add your greeting: record a WAV file, name it exactly this, and set
        // "Copy to Output Directory → Copy Always" in Visual Studio's file properties.
        private const string WavFileName = "greeting.wav";

        /// <summary>
        /// Attempts to play the WAV greeting file synchronously (blocking until audio finishes).
        /// Fails silently if the file is missing, the platform is non-Windows, or any error occurs.
        /// This ensures the chatbot always starts regardless of audio availability.
        /// </summary>
        public static void Play()
        {
            // Build the absolute path to the WAV file next to the compiled executable
            string wavPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WavFileName);

            // Guard: if the file does not exist, skip audio and continue
            if (!File.Exists(wavPath))
                return;

            try
            {
                // Use Reflection to load SoundPlayer at runtime rather than compile time.
                // This prevents a compile error on non-Windows platforms where
                // System.Windows.Extensions (which contains SoundPlayer) is unavailable.
                var soundPlayerType = Type.GetType(
                    "System.Media.SoundPlayer, System.Windows.Extensions");

                // Guard: SoundPlayer not available on this platform (e.g. macOS / Linux)
                if (soundPlayerType == null)
                    return;

                // Dynamically create: new SoundPlayer(wavPath)
                var player = Activator.CreateInstance(soundPlayerType, wavPath);

                // Dynamically call: player.PlaySync()
                // PlaySync blocks until the audio finishes — ensuring the greeting
                // completes before the logo and chatbot prompt appear on screen.
                var playSync = soundPlayerType.GetMethod("PlaySync");
                playSync?.Invoke(player, null);
            }
            catch
            {
                // Swallow ALL audio exceptions.
                // Audio failure must never prevent the chatbot from running.
            }
        }
    }
}

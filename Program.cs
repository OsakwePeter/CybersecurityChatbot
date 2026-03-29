// ============================================================
//  CybersecurityChatbot — Part 1
//  Module : PROG6221/w  Programming 2A
//  Purpose: A console-based Cybersecurity Awareness Assistant
//           that educates South African citizens on staying
//           safe online through interactive conversation.
// ============================================================

using System;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Application entry point.
    /// Coordinates the full startup sequence in order:
    ///   1. Voice greeting  — plays a recorded WAV welcome message
    ///   2. ASCII art logo  — displays the branded title screen
    ///   3. Chatbot session — runs the interactive conversation loop
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main method — the first method executed when the application launches.
        /// Delegates all work to specialised classes to maintain separation of concerns.
        /// </summary>
        /// <param name="args">Command-line arguments (not used in this application).</param>
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // 1. Play the recorded voice greeting (Windows only — fails silently on other platforms)
            VoiceGreeting.Play();

            // 2. Clear the screen and display the ASCII art logo and application branding
            AsciiArt.Display();

            // 3. Instantiate the chatbot and begin the interactive conversation session
            Chatbot chatbot = new Chatbot();
            chatbot.Start();
        }
    }
}

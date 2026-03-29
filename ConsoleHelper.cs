// ============================================================
//  ConsoleHelper.cs
//  Responsibility: All console output styling — colours,
//                 borders, dividers, typing effects, and
//                 labelled message prefixes.
//  Design       : Centralising formatting here means the
//                 visual style can be changed in one place
//                 without touching business logic files.
//                 (DRY — Don't Repeat Yourself principle)
// ============================================================

using System;
using System.Threading;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Provides reusable console formatting utilities used throughout the application.
    /// Includes colour helpers, decorative borders, a typewriter effect, and
    /// standardised bot/user message labels.
    /// Static class — no instantiation required.
    /// </summary>
    public static class ConsoleHelper
    {
        // ── Colour helpers ────────────────────────────────────────────────────

        /// <summary>
        /// Sets the console foreground colour without printing anything.
        /// Caller is responsible for calling Console.ResetColor() afterwards.
        /// </summary>
        /// <param name="color">The ConsoleColor to apply.</param>
        public static void SetColor(ConsoleColor color)
            => Console.ForegroundColor = color;

        /// <summary>
        /// Prints text in the specified colour on the current line (no newline).
        /// Resets colour immediately after printing so subsequent output is unaffected.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="color">Colour to use.</param>
        public static void WriteColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints text in the specified colour followed by a newline.
        /// Resets colour immediately after printing so subsequent output is unaffected.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="color">Colour to use.</param>
        public static void WriteLineColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        // ── Borders and dividers ──────────────────────────────────────────────

        /// <summary>
        /// Prints a heavy horizontal border using '═' characters in DarkCyan.
        /// Used for major section separators (e.g. around the logo, start/end of session).
        /// </summary>
        /// <param name="width">Number of characters wide. Defaults to 60.</param>
        public static void PrintBorder(int width = 60)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('═', width));
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a light horizontal divider using '─' characters in DarkGray.
        /// Used between individual conversation turns for readability.
        /// </summary>
        /// <param name="width">Number of characters wide. Defaults to 60.</param>
        public static void PrintDivider(int width = 60)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', width));
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a titled section header wrapped in heavy borders.
        /// Useful for clearly labelling distinct areas of the console output.
        /// </summary>
        /// <param name="title">The header text to display.</param>
        public static void PrintSectionHeader(string title)
        {
            Console.WriteLine();
            PrintBorder(60);
            WriteLineColored($"  {title}", ConsoleColor.Yellow);
            PrintBorder(60);
        }

        // ── Typing / animation effect ─────────────────────────────────────────

        /// <summary>
        /// Prints text character-by-character with a configurable delay between each character,
        /// simulating a typewriter / live-typing effect for a conversational feel.
        /// After all characters are printed, moves to a new line.
        /// </summary>
        /// <param name="text">The text to animate.</param>
        /// <param name="color">Text colour. Defaults to White.</param>
        /// <param name="delayMs">Milliseconds between characters. Defaults to 18ms.</param>
        public static void TypeWrite(string text,
                                     ConsoleColor color = ConsoleColor.White,
                                     int delayMs = 18)
        {
            Console.ForegroundColor = color;

            // Iterate character by character — strings are arrays of chars in C#
            foreach (char c in text)
            {
                Console.Write(c);           // Print the character without newline
                Thread.Sleep(delayMs);      // Pause briefly to create the typing effect
            }

            Console.ResetColor();
            Console.WriteLine();            // Move to the next line after the full message
        }

        // ── Message labels ────────────────────────────────────────────────────

        /// <summary>
        /// Prints the bot's name label in green on the current line (no newline).
        /// The response text is printed immediately after on the same line.
        /// Example output:  🤖 CyberBot: [response follows here]
        /// </summary>
        public static void PrintBotLabel()
        {
            WriteColored(" 🤖 CyberBot: ", ConsoleColor.Green);
        }

        /// <summary>
        /// Prints the user's name label in cyan on the current line (no newline).
        /// The user's input is printed immediately after on the same line.
        /// Example output:  👤 Sipho: [user input follows here]
        /// </summary>
        /// <param name="name">The user's name to display in the label.</param>
        public static void PrintUserLabel(string name)
        {
            WriteColored($" 👤 {name}: ", ConsoleColor.Cyan);
        }

        // ── Status / feedback messages ────────────────────────────────────────

        /// <summary>
        /// Prints a warning message in DarkYellow with a ⚠️ prefix.
        /// Used for input validation errors and non-critical alerts.
        /// </summary>
        /// <param name="message">Warning message to display.</param>
        public static void PrintWarning(string message)
        {
            WriteLineColored($" ⚠️  {message}", ConsoleColor.DarkYellow);
        }

        /// <summary>
        /// Prints an error message in Red with a ✖ prefix.
        /// Used for critical validation failures such as empty name entry.
        /// </summary>
        /// <param name="message">Error message to display.</param>
        public static void PrintError(string message)
        {
            WriteLineColored($" ✖  {message}", ConsoleColor.Red);
        }

        /// <summary>
        /// Prints a success or informational message in Green with a ✔ prefix.
        /// Used for positive confirmations.
        /// </summary>
        /// <param name="message">Success message to display.</param>
        public static void PrintSuccess(string message)
        {
            WriteLineColored($" ✔  {message}", ConsoleColor.Green);
        }
    }
}

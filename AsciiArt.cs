// ============================================================
//  AsciiArt.cs
//  Responsibility: Displays the branded ASCII art title screen
//                 when the application launches.
//  Design       : Uses Unicode box-drawing characters for a
//                 polished, professional appearance.
//                 Colour scheme: Cyan logo → Yellow subtitle
//                 → Green tip → reset to default.
// ============================================================

using System;
using System.Threading;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Displays the ASCII art logo, application branding, and a
    /// rotating security tip on the title screen at startup.
    /// Static class — no instantiation required.
    /// </summary>
    public static class AsciiArt
    {
        // ── Security tips shown randomly on the title screen ─────────────────
        // Adds variety and immediate educational value before the user even types anything.
        private static readonly string[] SecurityTips = new[]
        {
            "💡 Tip: Never reuse passwords across different websites.",
            "💡 Tip: Enable two-factor authentication on all important accounts.",
            "💡 Tip: Think before you click — verify links before opening them.",
            "💡 Tip: Keep your software and operating system up to date.",
            "💡 Tip: Use a password manager to generate and store strong passwords.",
            "💡 Tip: Back up your data regularly using the 3-2-1 rule.",
            "💡 Tip: Avoid accessing sensitive accounts on public Wi-Fi.",
        };

        /// <summary>
        /// Clears the console and renders the full title screen including:
        /// ASCII art logo, application title, tagline, a random security tip,
        /// and a decorative border. Pauses briefly so the user can read it.
        /// </summary>
        public static void Display()
        {
            Console.Clear();

            // ── Top decorative border ─────────────────────────────────────────
            ConsoleHelper.PrintBorder(70);

            // ── ASCII art logo (generated using patorjk.com — ANSI Shadow font) ──
            Console.ForegroundColor = ConsoleColor.Cyan;
            string[] logo = new[]
            {
                @"  ██████╗██╗   ██╗██████╗ ███████╗██████╗ ███████╗███████╗ ██████╗",
                @" ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔════╝██╔════╝██╔════╝",
                @" ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝███████╗█████╗  ██║     ",
                @" ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗╚════██║██╔══╝  ██║     ",
                @" ╚██████╗   ██║   ██████╔╝███████╗██║  ██║███████║███████╗╚██████╗",
                @"  ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝╚══════╝ ╚═════╝",
            };

            foreach (var line in logo)
                Console.WriteLine(line);

            Console.ResetColor();

            // ── Middle border ─────────────────────────────────────────────────
            ConsoleHelper.PrintBorder(70);

            // ── Shield icon and application title ────────────────────────────
            Console.WriteLine();
            ConsoleHelper.WriteLineColored(
                "              🛡️  CYBERSECURITY AWARENESS BOT  🛡️",
                ConsoleColor.Yellow);
            ConsoleHelper.WriteLineColored(
                "          Your trusted guide to staying safe online in SA",
                ConsoleColor.White);
            Console.WriteLine();

            // ── Random security tip — educational value from the first second ─
            ConsoleHelper.PrintDivider(70);
            string tip = GetRandomTip();
            ConsoleHelper.WriteLineColored($"  {tip}", ConsoleColor.Green);
            ConsoleHelper.PrintDivider(70);

            Console.WriteLine();

            // ── Bottom border ─────────────────────────────────────────────────
            ConsoleHelper.PrintBorder(70);
            Console.ResetColor();
            Console.WriteLine();

            // Brief pause so the user can read the title screen before the prompt appears
            Thread.Sleep(500);
        }

        // ── Private helpers ───────────────────────────────────────────────────

        /// <summary>
        /// Returns a randomly selected security tip from the tips array.
        /// Adds variety to the title screen on each launch.
        /// </summary>
        private static string GetRandomTip()
        {
            Random random = new Random();
            int index = random.Next(0, SecurityTips.Length);
            return SecurityTips[index];
        }
    }
}

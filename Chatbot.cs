// ============================================================
//  Chatbot.cs
//  Responsibility: Orchestrates the full conversation session.
//                 Handles user greeting, name collection,
//                 the main input/response loop, input
//                 validation, and session teardown.
//  Dependencies : ResponseEngine  — provides keyword responses
//                 ConsoleHelper   — all console formatting
// ============================================================

using System;
using System.Threading;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Manages the complete chatbot conversation session from welcome to farewell.
    /// Responsibilities:
    ///   • Greeting the user and collecting their name
    ///   • Running the main conversation loop
    ///   • Validating all user input before processing
    ///   • Routing input to ResponseEngine and displaying results
    ///   • Detecting exit commands and closing the session gracefully
    /// </summary>
    public class Chatbot
    {
        // ── Private fields ────────────────────────────────────────────────────

        /// <summary>The response engine that matches user input to cybersecurity responses.</summary>
        private readonly ResponseEngine _engine = new ResponseEngine();

        /// <summary>The user's name — collected at startup and used throughout for personalisation.</summary>
        private string _userName = "there";

        /// <summary>
        /// Controls whether the conversation loop continues running.
        /// Set to false by exit commands or EOF to end the session cleanly.
        /// </summary>
        private bool _sessionActive = true;

        // ── Public API ────────────────────────────────────────────────────────

        /// <summary>
        /// Starts the full chatbot session.
        /// Runs welcome → conversation loop → farewell in sequence.
        /// This is the only public method — all other methods are private implementation details.
        /// </summary>
        public void Start()
        {
            WelcomeUser();
            RunConversationLoop();
            PrintFarewell();
        }

        // ── Private methods ───────────────────────────────────────────────────

        /// <summary>
        /// Displays the welcome message, collects and validates the user's name,
        /// then prints a personalised greeting with usage instructions.
        /// Uses a while loop to keep asking until a valid (non-empty) name is entered.
        /// </summary>
        private void WelcomeUser()
        {
            // Display welcome heading
            ConsoleHelper.WriteLineColored(
                " Welcome to the Cybersecurity Awareness Bot!",
                ConsoleColor.Yellow);
            ConsoleHelper.WriteLineColored(
                " Empowering South African citizens to stay safe online. 🛡️",
                ConsoleColor.White);
            ConsoleHelper.PrintDivider();
            Console.WriteLine();

            // ── Name collection with validation loop ──────────────────────────
            // Keep asking until the user provides a non-empty name
            ConsoleHelper.WriteColored(" Please enter your name: ", ConsoleColor.Cyan);
            string input = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(input))
            {
                // Input was empty or whitespace — show error and ask again
                ConsoleHelper.PrintError("Name cannot be empty. Please try again.");
                Console.WriteLine();
                ConsoleHelper.WriteColored(" Please enter your name: ", ConsoleColor.Cyan);
                input = Console.ReadLine();
            }

            // Store the trimmed name and pass it to the response engine for personalisation
            _userName = input.Trim();
            _engine.SetUserName(_userName);

            // ── Personalised greeting ─────────────────────────────────────────
            Console.WriteLine();
            ConsoleHelper.PrintBorder(60);
            Console.WriteLine();

            // Greeting line 1
            ConsoleHelper.PrintBotLabel();
            ConsoleHelper.TypeWrite(
                $"Hello, {_userName}! 👋 Great to meet you.",
                ConsoleColor.Green);

            Thread.Sleep(300);

            // Greeting line 2 — explain the bot's purpose
            ConsoleHelper.PrintBotLabel();
            ConsoleHelper.TypeWrite(
                "I'm your Cybersecurity Awareness Assistant, here to help " +
                "you navigate the digital world safely.",
                ConsoleColor.Green);

            Thread.Sleep(300);

            // Greeting line 3 — topics overview
            ConsoleHelper.PrintBotLabel();
            ConsoleHelper.TypeWrite(
                "I can help you with: phishing, passwords, safe browsing, " +
                "malware, social engineering, 2FA, and much more.",
                ConsoleColor.Green);

            Thread.Sleep(300);

            // Greeting line 4 — how to get started
            ConsoleHelper.PrintBotLabel();
            ConsoleHelper.TypeWrite(
                "Type 'menu' to see all topics, 'help' for example questions, " +
                "or 'exit' to leave.",
                ConsoleColor.Green);

            Console.WriteLine();
            ConsoleHelper.PrintBorder(60);
            Console.WriteLine();
        }

        /// <summary>
        /// The main conversation loop — runs continuously while _sessionActive is true.
        /// Each iteration:
        ///   1. Displays the user prompt and reads input
        ///   2. Validates the input (skips to next iteration if invalid)
        ///   3. Checks for exit commands (breaks the loop if detected)
        ///   4. Gets a response from the engine (or default fallback)
        ///   5. Displays the response with a typing animation
        /// </summary>
        private void RunConversationLoop()
        {
            while (_sessionActive)
            {
                // Display user label and read their input
                ConsoleHelper.PrintUserLabel(_userName);
                string rawInput = Console.ReadLine();

                Console.WriteLine();

                // Validate the raw input — 'out' parameter receives the cleaned version
                // 'continue' skips the rest of this iteration if validation fails
                if (!ValidateInput(rawInput, out string cleanedInput))
                    continue;

                // Check if the user wants to end the session
                if (IsExitCommand(cleanedInput))
                {
                    _sessionActive = false;

                    // Get the goodbye response from the engine, or use a built-in fallback
                    // The ?? (null coalescing) operator returns the right side if left is null
                    string exitResponse = _engine.GetResponse("exit")
                        ?? $"Goodbye, {_userName}! Stay safe online! 🛡️";

                    ConsoleHelper.PrintBotLabel();
                    ConsoleHelper.TypeWrite(exitResponse, ConsoleColor.Green);
                    Console.WriteLine();
                    break; // Exit the while loop immediately
                }

                // ── Get and display the response ──────────────────────────────
                // Try to find a keyword match; fall back to default if none found
                string response = _engine.GetResponse(cleanedInput)
                    ?? BuildDefaultResponse(cleanedInput);

                ConsoleHelper.PrintBotLabel();
                ConsoleHelper.TypeWrite(response, ConsoleColor.Green, delayMs: 12);

                Console.WriteLine();
                ConsoleHelper.PrintDivider(60);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Validates raw console input against three rules:
        ///   1. Not null (EOF / stream closed)
        ///   2. Not empty or whitespace only
        ///   3. Not excessively long (over 500 characters)
        /// Uses an 'out' parameter to return the cleaned input alongside the bool result,
        /// allowing the caller to receive both the validation result and cleaned value in one call.
        /// </summary>
        /// <param name="raw">The raw string read from Console.ReadLine().</param>
        /// <param name="cleaned">
        /// Output parameter — contains the trimmed input if valid, or empty string if invalid.
        /// </param>
        /// <returns>True if the input is valid and safe to process; false otherwise.</returns>
        private bool ValidateInput(string raw, out string cleaned)
        {
            // 'out' parameters must be assigned before the method can return
            cleaned = string.Empty;

            // Rule 1: null means the input stream was closed (e.g. piped input ended)
            if (raw == null)
            {
                _sessionActive = false; // End the session cleanly
                return false;
            }

            // Rule 2: empty or whitespace-only input is not meaningful
            if (string.IsNullOrWhiteSpace(raw))
            {
                ConsoleHelper.PrintWarning("Please type a message before pressing Enter.");
                Console.WriteLine();
                return false;
            }

            // Rule 3: prevent unreasonably long inputs
            if (raw.Trim().Length > 500)
            {
                ConsoleHelper.PrintWarning(
                    "That message is too long. Please keep input under 500 characters.");
                Console.WriteLine();
                return false;
            }

            // All checks passed — assign the trimmed input to the out parameter
            cleaned = raw.Trim();
            return true;
        }

        /// <summary>
        /// Checks whether the user's input is one of the recognised exit commands.
        /// Comparison is case-insensitive and requires an exact match (not a substring).
        /// Exact matching prevents accidental exits if someone types "goodbye phishing".
        /// </summary>
        /// <param name="input">The cleaned user input to check.</param>
        /// <returns>True if the input is an exit command; false otherwise.</returns>
        private static bool IsExitCommand(string input)
        {
            string lower = input.ToLower();

            // || means OR — returns true if ANY of these conditions is true
            return lower == "exit"
                || lower == "quit"
                || lower == "bye"
                || lower == "goodbye"
                || lower == "q";
        }

        /// <summary>
        /// Builds a helpful default response when no keyword match is found in ResponseEngine.
        /// Echoes the user's input back to them and suggests how to find valid topics,
        /// making the fallback informative rather than just a generic error.
        /// </summary>
        /// <param name="input">The user's unrecognised input.</param>
        /// <returns>A friendly default response string.</returns>
        private string BuildDefaultResponse(string input)
        {
            return $"I didn't quite understand \"{input}\". Could you rephrase? 🤔\n" +
                   $"  → Type 'menu' to see all available topics\n" +
                   $"  → Type 'help' for example questions you can ask";
        }

        /// <summary>
        /// Displays the closing farewell message after the conversation loop ends.
        /// Summarises key cybersecurity reminders to reinforce learning before the user leaves.
        /// </summary>
        private void PrintFarewell()
        {
            Console.WriteLine();
            ConsoleHelper.PrintBorder(60);
            Console.WriteLine();

            ConsoleHelper.WriteLineColored(
                $"  Thanks for using the Cybersecurity Awareness Bot, {_userName}!",
                ConsoleColor.Yellow);

            Console.WriteLine();

            // Quick security reminder checklist — reinforce key lessons
            ConsoleHelper.WriteLineColored("  Remember to stay safe online:", ConsoleColor.White);
            ConsoleHelper.WriteLineColored("    🔐  Use strong, unique passwords for every account", ConsoleColor.Cyan);
            ConsoleHelper.WriteLineColored("    📲  Enable two-factor authentication everywhere", ConsoleColor.Cyan);
            ConsoleHelper.WriteLineColored("    🎣  Think before you click on any link or attachment", ConsoleColor.Cyan);
            ConsoleHelper.WriteLineColored("    💾  Back up your important data regularly", ConsoleColor.Cyan);

            Console.WriteLine();
            ConsoleHelper.WriteLineColored("  Stay vigilant. Stay secure. 🛡️", ConsoleColor.Green);

            Console.WriteLine();
            ConsoleHelper.PrintBorder(60);
            Console.WriteLine();
        }
    }
}

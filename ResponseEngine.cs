// ============================================================
//  ResponseEngine.cs
//  Responsibility: The knowledge base of the chatbot.
//                 Maps cybersecurity keywords to educational
//                 responses. Handles keyword matching,
//                 name personalisation, and fallback logic.
//  Pattern      : Simple keyword-based NLP — converts input
//                 to lowercase then checks if it contains
//                 any registered keyword using Dictionary lookup.
// ============================================================

using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Provides keyword-matched cybersecurity responses for the chatbot.
    /// Maintains a Dictionary of keyword → response pairs covering 20+ cybersecurity topics.
    /// Personalises responses by replacing the {name} placeholder with the user's actual name.
    /// </summary>
    public class ResponseEngine
    {
        // ── Private fields ────────────────────────────────────────────────────

        /// <summary>
        /// The core knowledge base: maps keyword strings to full response strings.
        /// readonly — the dictionary reference cannot be replaced after construction.
        /// Built once in the constructor via BuildResponses().
        /// </summary>
        private readonly Dictionary<string, string> _responses;

        /// <summary>
        /// The current user's name used to personalise responses.
        /// Defaults to "there" so responses make sense before SetUserName() is called.
        /// Example: "Hi {name}!" becomes "Hi there!" until a real name is provided.
        /// </summary>
        private string _userName = "there";

        // ── Constructor ───────────────────────────────────────────────────────

        /// <summary>
        /// Initialises the ResponseEngine by building the complete response dictionary.
        /// Called automatically when: ResponseEngine engine = new ResponseEngine();
        /// </summary>
        public ResponseEngine()
        {
            _responses = BuildResponses();
        }

        // ── Public methods ────────────────────────────────────────────────────

        /// <summary>
        /// Updates the stored user name used for response personalisation.
        /// Should be called once after the user enters their name in Chatbot.cs.
        /// Arrow (=>) syntax is shorthand for a single-line method body.
        /// </summary>
        /// <param name="name">The user's name to embed in responses.</param>
        public void SetUserName(string name) => _userName = name;

        /// <summary>
        /// Finds and returns the best matching response for the given user input.
        /// Matching strategy:
        ///   1. Convert input to lowercase (case-insensitive matching)
        ///   2. Loop through every keyword in the dictionary
        ///   3. Return the first response whose keyword appears anywhere in the input
        ///   4. Replace {name} placeholder with the actual user name before returning
        /// Returns null if no keyword matches — caller should use a default response.
        /// </summary>
        /// <param name="input">The raw user input string to match against.</param>
        /// <returns>A personalised response string, or null if no match found.</returns>
        public string? GetResponse(string input)
        {
            // Guard: empty or whitespace input cannot match any keyword
            if (string.IsNullOrWhiteSpace(input))
                return null;

            // Normalise to lowercase so matching is case-insensitive
            // e.g. "PHISHING", "Phishing", "phishing" all match keyword "phish"
            string lower = input.ToLower();

            // Search the dictionary for a keyword contained in the input
            foreach (var kvp in _responses)
            {
                // kvp.Key = keyword (e.g. "phish")
                // kvp.Value = full response text
                // Contains() checks if the keyword appears ANYWHERE in the input
                if (lower.Contains(kvp.Key))
                {
                    // Replace the {name} placeholder with the actual user name
                    // e.g. "Hello {name}!" → "Hello Sipho!"
                    return kvp.Value.Replace("{name}", _userName);
                }
            }

            // No keyword matched — return null so the caller can show a default message
            return null;
        }

        // ── Private methods ───────────────────────────────────────────────────

        /// <summary>
        /// Builds and returns the complete cybersecurity response dictionary.
        /// Each entry maps a lowercase keyword to a full educational response.
        /// Topics covered: greetings, phishing, passwords, browsing, malware,
        /// ransomware, social engineering, 2FA, Wi-Fi, VPN, backup, privacy,
        /// scams, updates, firewall, encryption, South Africa resources, farewells.
        /// StringComparer.OrdinalIgnoreCase adds an extra layer of case-insensitive
        /// key lookup safety at the dictionary level.
        /// </summary>
        /// <returns>A populated Dictionary mapping keywords to response strings.</returns>
        private static Dictionary<string, string> BuildResponses()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // ── Greetings ─────────────────────────────────────────────────
                {
                    "hello",
                    "Hi {name}! 👋 Great to have you here. " +
                    "I'm your Cybersecurity Awareness Bot. Ask me anything about staying safe online!"
                },
                {
                    "hi",
                    "Hey {name}! 😊 Ready to boost your cybersecurity knowledge? Just ask me a question!"
                },
                {
                    "how are you",
                    "I'm fully patched and running with no vulnerabilities detected! 😄 " +
                    "Thanks for asking, {name}. How can I help keep you safe online today?"
                },
                {
                    "good morning",
                    "Good morning, {name}! ☀️ Start your day securely. " +
                    "Remember to check for software updates before you begin work!"
                },
                {
                    "good afternoon",
                    "Good afternoon, {name}! 🌤️ " +
                    "A good time to review your recent login activity and ensure everything looks normal."
                },
                {
                    "good evening",
                    "Good evening, {name}! 🌙 " +
                    "Quick tip before you wind down: log out of shared devices and lock your screen."
                },

                // ── Bot purpose / capabilities ────────────────────────────────
                {
                    "purpose",
                    "My purpose is to educate you on cybersecurity threats and safe online practices. " +
                    "I can help with topics like phishing, passwords, malware, safe browsing, and more! 🛡️"
                },
                {
                    "what can you do",
                    "Great question, {name}! Here's what I can help with:\n" +
                    "  🎣  Phishing awareness\n" +
                    "  🔐  Password best practices\n" +
                    "  🌐  Safe browsing habits\n" +
                    "  🦠  Malware and virus protection\n" +
                    "  📱  Social engineering defence\n" +
                    "  🔒  Two-factor authentication\n" +
                    "  🛡️  General cybersecurity advice\n" +
                    "Just ask about any of these topics!"
                },
                {
                    "what can i ask",
                    "You can ask me about:\n" +
                    "  → Phishing      → Passwords       → Safe Browsing\n" +
                    "  → Malware       → Social Engineering  → 2FA\n" +
                    "  → Public Wi-Fi  → Data backup     → Privacy\n" +
                    "Give any of those a try, {name}!"
                },
                {
                    "help",
                    "Sure, {name}! Here are some example questions:\n" +
                    "  🎣  'Tell me about phishing'\n" +
                    "  🔐  'How do I create a strong password?'\n" +
                    "  🌐  'What is safe browsing?'\n" +
                    "  🦠  'What is malware?'\n" +
                    "  📲  'What is two-factor authentication?'\n" +
                    "  🔌  'Is public Wi-Fi safe?'\n" +
                    "Type 'menu' to see a full topic list."
                },
                {
                    "menu",
                    "╔══════════════════════════════════╗\n" +
                    "║         TOPIC MENU               ║\n" +
                    "╠══════════════════════════════════╣\n" +
                    "║  1.  Phishing                    ║\n" +
                    "║  2.  Password safety             ║\n" +
                    "║  3.  Safe browsing               ║\n" +
                    "║  4.  Malware                     ║\n" +
                    "║  5.  Ransomware                  ║\n" +
                    "║  6.  Social engineering          ║\n" +
                    "║  7.  Two-factor authentication   ║\n" +
                    "║  8.  Public Wi-Fi / VPN          ║\n" +
                    "║  9.  Data backup                 ║\n" +
                    "║  10. Privacy                     ║\n" +
                    "║  11. Scams                       ║\n" +
                    "║  12. Software updates            ║\n" +
                    "║  13. Firewall                    ║\n" +
                    "║  14. Encryption                  ║\n" +
                    "║  15. South Africa resources      ║\n" +
                    "╚══════════════════════════════════╝\n" +
                    "Type any topic name to learn more!"
                },

                // ── Phishing ──────────────────────────────────────────────────
                {
                    "phish",
                    "🎣 Phishing is a cyber-attack where criminals impersonate trustworthy " +
                    "sources to steal sensitive information like passwords or banking details.\n\n" +
                    "🔎 How to spot a phishing email:\n" +
                    "  ✔ Check the sender's FULL email address carefully — not just the display name\n" +
                    "  ✔ Look for spelling mistakes, urgency tactics, and unusual requests\n" +
                    "  ✔ Hover over links before clicking — never click suspicious URLs\n" +
                    "  ✔ Legitimate companies will never ask for your password via email\n" +
                    "  ✔ Be suspicious of unexpected attachments, even from known contacts\n\n" +
                    "When in doubt: go directly to the official website rather than clicking any link."
                },

                // ── Password ──────────────────────────────────────────────────
                {
                    "password",
                    "🔐 Strong passwords are your first line of defence!\n\n" +
                    "✅ Best practices:\n" +
                    "  • Use at least 12 characters\n" +
                    "  • Mix uppercase, lowercase, numbers, and symbols\n" +
                    "  • Never reuse passwords across different sites\n" +
                    "  • Avoid personal info (birthdays, names, pet names)\n" +
                    "  • Use a reputable password manager (e.g. Bitwarden, 1Password)\n" +
                    "  • Change passwords immediately if a breach is suspected\n\n" +
                    "💡 Tip for {name}: A passphrase like 'Coffee@Sunrise!42' is both strong and memorable."
                },

                // ── Safe browsing ─────────────────────────────────────────────
                {
                    "brows",
                    "🌐 Safe Browsing Tips for {name}:\n\n" +
                    "  🔒 Always look for 'https://' and the padlock icon in the address bar\n" +
                    "  🚫 Avoid clicking pop-up ads or suspicious download buttons\n" +
                    "  🧩 Keep your browser and extensions up to date\n" +
                    "  🛡️ Use an ad blocker and privacy-focused extensions\n" +
                    "  🔍 Verify URLs carefully — scammers use lookalike domains (e.g. paypa1.com)\n" +
                    "  📥 Only download software from official, trusted sources\n" +
                    "  🍪 Clear cookies and cache regularly to protect your session data"
                },
                {
                    "https",
                    "🔒 HTTPS (HyperText Transfer Protocol Secure) encrypts the data exchanged " +
                    "between your browser and a website, protecting it from interception.\n" +
                    "Always check for 'https://' and a padlock before entering any sensitive information.\n" +
                    "HTTP without the 'S' means the connection is unencrypted — avoid entering passwords there."
                },

                // ── Malware ───────────────────────────────────────────────────
                {
                    "malware",
                    "🦠 Malware is malicious software designed to damage systems or steal data.\n\n" +
                    "Common types:\n" +
                    "  • Viruses      — self-replicating programs that corrupt files\n" +
                    "  • Ransomware   — encrypts your data and demands payment\n" +
                    "  • Spyware      — secretly monitors your activity and keystrokes\n" +
                    "  • Trojans      — disguise themselves as legitimate software\n" +
                    "  • Adware       — displays unwanted ads and tracks behaviour\n\n" +
                    "🛡️ Protection tips:\n" +
                    "  ✔ Install reputable antivirus software and keep it updated\n" +
                    "  ✔ Keep your OS and all applications patched\n" +
                    "  ✔ Never download software from unknown or untrusted sources\n" +
                    "  ✔ Scan USB drives before opening files on them"
                },
                {
                    "virus",
                    "🦠 A computer virus attaches itself to legitimate files and spreads when " +
                    "those files are shared. Use up-to-date antivirus software and avoid " +
                    "opening email attachments from unknown senders."
                },
                {
                    "ransomware",
                    "💰 Ransomware is a devastating attack — criminals encrypt your files " +
                    "and demand payment to restore access.\n\n" +
                    "Prevention:\n" +
                    "  ✔ Back up data regularly — offline AND cloud copies\n" +
                    "  ✔ Keep all software patched and updated\n" +
                    "  ✔ Never pay the ransom — it doesn't guarantee recovery\n" +
                    "  ✔ Report incidents to your country's cybercrime authority"
                },

                // ── Social engineering ────────────────────────────────────────
                {
                    "social engineer",
                    "🎭 Social engineering manipulates people psychologically to reveal " +
                    "confidential information — it exploits human trust, not technology.\n\n" +
                    "Common tactics:\n" +
                    "  • Pretexting  — creating a fake scenario to extract info\n" +
                    "  • Baiting     — leaving infected USB drives in public areas\n" +
                    "  • Tailgating  — following someone into a secure area\n" +
                    "  • Vishing     — voice phishing over the telephone\n\n" +
                    "🛡️ Defence: Always verify identities, question unexpected requests, " +
                    "and never give sensitive information under pressure."
                },

                // ── Two-factor authentication ─────────────────────────────────
                {
                    "two-factor",
                    "📲 Two-Factor Authentication (2FA) adds a second layer of security " +
                    "beyond your password.\n\n" +
                    "How it works:\n" +
                    "  1️⃣  Enter your password\n" +
                    "  2️⃣  Confirm your identity via SMS, authenticator app, or hardware key\n\n" +
                    "Even if your password is stolen, 2FA blocks the attacker from logging in.\n" +
                    "Enable 2FA on ALL important accounts — especially email and banking!"
                },
                {
                    "2fa",
                    "📲 2FA (Two-Factor Authentication) dramatically reduces account hijacking risk.\n" +
                    "Use an authenticator app (Google Authenticator, Microsoft Authenticator) " +
                    "rather than SMS when possible — SMS can be intercepted via SIM-swapping attacks."
                },
                {
                    "authenticat",
                    "📲 Strong authentication uses multiple factors:\n" +
                    "  🔑 Something you KNOW   — password or PIN\n" +
                    "  📱 Something you HAVE   — phone or security token\n" +
                    "  👁️ Something you ARE    — fingerprint or face scan\n" +
                    "Enable multi-factor authentication (MFA) wherever it is available!"
                },

                // ── Public Wi-Fi ──────────────────────────────────────────────
                {
                    "wi-fi",
                    "📡 Public Wi-Fi risks:\n" +
                    "  ⚠️  Evil-twin hotspots can mimic legitimate network names\n" +
                    "  ⚠️  Man-in-the-middle attacks can intercept your traffic\n" +
                    "  ⚠️  Unencrypted connections expose passwords and personal data\n\n" +
                    "🛡️ Stay safe:\n" +
                    "  ✔ Always use a VPN when on public networks\n" +
                    "  ✔ Avoid accessing banking or sensitive accounts on public Wi-Fi\n" +
                    "  ✔ Forget the network when you're done\n" +
                    "  ✔ Use your mobile data instead whenever possible"
                },
                {
                    "wifi",
                    "📡 Public Wi-Fi is risky. Always use a VPN when connecting to " +
                    "public networks and avoid accessing sensitive accounts like banking."
                },
                {
                    "vpn",
                    "🔐 A VPN (Virtual Private Network) encrypts your internet connection " +
                    "and masks your IP address, protecting your privacy on any network.\n" +
                    "It's especially important on public Wi-Fi.\n" +
                    "Choose a reputable PAID VPN — free VPNs often monetise your data instead."
                },

                // ── Data backup ───────────────────────────────────────────────
                {
                    "backup",
                    "💾 Regular backups are your safety net against ransomware, hardware failure, " +
                    "and accidental deletion.\n\n" +
                    "Follow the 3-2-1 rule:\n" +
                    "  3️⃣  Keep 3 copies of your data\n" +
                    "  2️⃣  Store on 2 different media types (e.g. hard drive + cloud)\n" +
                    "  1️⃣  Keep 1 copy offsite or in the cloud\n\n" +
                    "⚠️  Test your backups regularly — a backup you can't restore is worthless!"
                },

                // ── Privacy ───────────────────────────────────────────────────
                {
                    "privacy",
                    "🔏 Protecting your privacy online:\n" +
                    "  • Review app permissions regularly — revoke what isn't needed\n" +
                    "  • Use privacy-focused browsers (Firefox, Brave)\n" +
                    "  • Enable private/incognito mode on shared devices\n" +
                    "  • Read privacy policies before sharing personal data\n" +
                    "  • Limit personal information shared on social media platforms"
                },
                {
                    "data",
                    "📊 Your personal data is valuable — protect it by:\n" +
                    "  ✔ Only sharing what is absolutely necessary\n" +
                    "  ✔ Using strong, unique passwords for every account\n" +
                    "  ✔ Checking if your email has been breached at haveibeenpwned.com\n" +
                    "  ✔ Reviewing connected app permissions on a regular basis"
                },

                // ── Scams ─────────────────────────────────────────────────────
                {
                    "scam",
                    "⚠️ Common online scams in South Africa:\n" +
                    "  • 'You've won!' lottery and prize scams\n" +
                    "  • Fake job offers requiring upfront payment\n" +
                    "  • Romance scams targeting people on dating platforms\n" +
                    "  • SARS (tax) impersonation emails and calls\n" +
                    "  • Banking app vishing (voice phishing) calls\n" +
                    "  • Fake online stores that take payment but deliver nothing\n\n" +
                    "Rule of thumb: if it sounds too good to be true, it almost certainly is!"
                },

                // ── Updates ───────────────────────────────────────────────────
                {
                    "update",
                    "🔄 Software updates patch known security vulnerabilities. " +
                    "Attackers actively exploit unpatched systems within hours of a vulnerability being published.\n\n" +
                    "Best practice:\n" +
                    "  ✔ Enable automatic updates for your OS and applications\n" +
                    "  ✔ Keep browsers and plugins updated — especially Java and PDF readers\n" +
                    "  ✔ Update your home router's firmware regularly"
                },

                // ── Firewall ──────────────────────────────────────────────────
                {
                    "firewall",
                    "🔥 A firewall monitors and controls incoming and outgoing network traffic " +
                    "based on predefined security rules. It acts as a barrier between your " +
                    "device and untrusted networks.\n" +
                    "Ensure your OS firewall is always enabled and consider a hardware " +
                    "firewall for your home network."
                },

                // ── Encryption ────────────────────────────────────────────────
                {
                    "encrypt",
                    "🔐 Encryption converts your data into an unreadable format that can " +
                    "only be decrypted with the correct key. Use it to protect:\n" +
                    "  • Files on your device  — BitLocker (Windows), FileVault (Mac)\n" +
                    "  • Communications        — Signal, WhatsApp end-to-end encryption\n" +
                    "  • Email                 — PGP encryption for sensitive messages"
                },

                // ── South Africa specific ─────────────────────────────────────
                {
                    "south africa",
                    "🇿🇦 South Africa has one of the highest cybercrime rates on the continent.\n\n" +
                    "Key local resources:\n" +
                    "  • Report cybercrime : www.cybercrime.org.za\n" +
                    "  • SAPS Cybercrime Unit : 10111\n" +
                    "  • SABRIC (South African Banking Risk Information Centre)\n" +
                    "  • POPIA (Protection of Personal Information Act) — protects your data rights\n" +
                    "  • Report fraud : www.justice.gov.za"
                },

                // ── Farewell ──────────────────────────────────────────────────
                {
                    "exit",
                    "Goodbye, {name}! 👋 Stay safe online. Remember:\n" +
                    "  🔐  Use strong, unique passwords\n" +
                    "  📲  Enable 2FA everywhere you can\n" +
                    "  🎣  Think before you click\n" +
                    "  💾  Back up your data regularly\n" +
                    "Come back anytime you need cybersecurity guidance! 🛡️"
                },
                {
                    "thank",
                    "You're welcome, {name}! 😊 " +
                    "Staying informed is your best defence online. " +
                    "Is there anything else you'd like to know?"
                },
            };
        }
    }
}

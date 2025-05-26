using System;
using System.Collections.Generic;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProgPart2
{
    internal class Response
    {
        private SoundPlayer errorPlayer;
        private SoundPlayer exitPlayer;
        private string userName;
        private string lastTopic = "";
        private string favoriteTopic = "";
        private int interactionCount = 0;

        private Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
        {
            { "password", new List<string> {
                "Always use strong passwords with a mix of letters, numbers, and symbols.",
                "Avoid reusing passwords across multiple platforms.",
                "Consider using a password manager to store and generate strong passwords."
            }},
            { "phishing", new List<string> {
                "Never click on suspicious links or attachments in emails.",
                "Check the sender’s email address carefully — phishing emails often imitate real companies.",
                "If something feels off, verify directly through official channels before acting."
            }},
            { "privacy", new List<string> {
                "Limit what personal information you share online.",
                "Review privacy settings on social media and apps regularly.",
                "Use encrypted messaging apps to protect your conversations."
            }},
            { "ransomware", new List<string> {
                "Keep regular backups of your important files in case of attack.",
                "Avoid downloading attachments or clicking links from unknown sources.",
                "Install and update antivirus software regularly to reduce the risk."
            }},
            { "ddos", new List<string> {
                "Use network firewalls and intrusion detection systems to help prevent DDoS attacks.",
                "Distribute network resources using a content delivery network (CDN) to manage traffic loads.",
                "Keep your infrastructure scalable to absorb traffic spikes from attacks."
            }},
            { "cybersecurity", new List<string> {
                "Cybersecurity includes protecting your networks, devices, and data from unauthorized access.",
                "Stay up to date on current threats and best practices to maintain digital safety.",
                "Regular software updates and awareness training are essential in cybersecurity."
            }},
            { "social engineering", new List<string> {
                "Always verify a person's identity before giving out information, even if they seem trustworthy.",
                "Be cautious of urgent requests for help or money — especially if asked over email or messaging.",
                "Never reveal sensitive info like passwords, OTPs, or banking info through calls or texts."
            }},
            { "zero day", new List<string> {
                "Zero-day vulnerabilities are flaws that hackers exploit before developers know or fix them.",
                "Keep your software updated so that once patches are released, you’re protected quickly.",
                "Use security tools that offer behavior analysis and threat detection for early signs of attack."
            }},
            { "safe browsing", new List<string> {
                "Use a secure connection (HTTPS), avoid clicking on suspicious links, and ensure your browser is up-to-date.",
                "Always verify URLs and avoid downloading from untrusted websites.",
                "Consider using browser security extensions and turn on automatic updates."
            }}
        };

        // ✅ Refactored: Now detects keywords anywhere in user input
        private Dictionary<string, string> fixedResponses = new Dictionary<string, string>()
        {
            //chatbot interations
            { "how are you", "I'm doing well, thank you! I'm always ready to help. What would you like to ask me about?" },
            { "purpose", "I'm here to help you stay safe online by providing tips on password security, phishing scams, and safe browsing practices." },
            //chatbot questions
            { "safe browsing", "Use a secure connection (HTTPS), avoid clicking on suspicious links, and ensure your browser is up-to-date." },
            { "suggestions", "Always use HTTPS websites, don’t click on suspicious links, and keep your browser updated." },
            { "ddos", "A DDoS (Distributed Denial of Service) attack is a cyberattack where a target is overwhelmed with traffic from multiple sources." },
            { "ransomware", "Ransomware is malware that locks or encrypts your data and demands payment to restore access." },
            { "six types", "Network security, application security, information security, cloud security, IoT security, and identity/access management." },
            { "zero day", "A zero-day vulnerability is a flaw unknown to the vendor, with no available patch." },
            { "social engineering", "Social engineering is the manipulation of people into revealing confidential information or performing unsafe actions." },
            { "cia triad", "The CIA triad stands for Confidentiality, Integrity, and Availability — the core principles of cybersecurity." },
            //questions to ask chatbot for questions
            { "ask about", "You can ask questions like: 'What is phishing?', 'How can I stay safe while browsing?', 'What are zero-day vulnerabilities?', 'How does ransomware work?', or 'What is social engineering?'" },
            { "what can i ask", "You can ask questions like: 'What is phishing?', 'How can I stay safe while browsing?', 'What are zero-day vulnerabilities?', 'How does ransomware work?', or 'What is social engineering?'" },
            { "give me some tips", "Sure! Here's a list of cybersecurity topics I can give you tips on: password safety, phishing, safe browsing, DDoS, ransomware, zero-day vulnerabilities, and social engineering. Just type the topic name!" },
            { "what are some tips", "Sure! Here's a list of cybersecurity topics I can give you tips on: password safety, phishing, safe browsing, DDoS, ransomware, zero-day vulnerabilities, and social engineering. Just type the topic name!" }
        };

        private Dictionary<string, string> sentimentResponses = new Dictionary<string, string>()
        {
            //emotions
            { "worried", "It's okay to feel worried — cybersecurity can be complex, but I'm here to guide you step-by-step." },
            { "frustrated", "I understand this can be frustrating. Take your time, and feel free to ask any question — no pressure!" },
            { "curious", "Curiosity is great! Let me help you explore the topic and learn something new." },
            { "overwhelmed", "Take a deep breath. You're doing great by asking questions. Let's tackle one thing at a time." },
            { "confused", "No worries — I’ll do my best to make things clearer for you. Just let me know what you’d like explained." }
        };

        public Response(string name)
        {
            userName = name;
            errorPlayer = new SoundPlayer(@"E:\Audio files\errorsound.wav");
            exitPlayer = new SoundPlayer(@"E:\Audio files\exit.wav");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ask me about password safety, phishing, or safe browsing.\nType 'what can I ask about' for suggestions, or 'quiz' to test your knowledge.");
            Console.WriteLine("\n_________________________________________________________");

            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"\n{userName}: ");
                    string userInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        PlayErrorSound();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Chatbot: Hmm... I didn't catch that. Could you rephrase?");
                        Thread.Sleep(1500);
                        continue;
                    }

                    userInput = Regex.Replace(userInput.ToLower(), @"[^\w\s]", "").Trim();
                    interactionCount++;

                    // Sentiment check
                    foreach (var sentiment in sentimentResponses)
                    {
                        if (userInput.Contains(sentiment.Key))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Chatbot: {sentiment.Value}");
                            Console.WriteLine("Chatbot: Would you like help with something specific like phishing or passwords?");
                            goto ContinuePrompt;
                        }
                    }

                    // Interest detection
                    Match interestMatch = Regex.Match(userInput, @"i(?: am|'m)? interested in (\w+)", RegexOptions.IgnoreCase);
                    if (interestMatch.Success)
                    {
                        string topic = interestMatch.Groups[1].Value.ToLower();
                        if (keywordResponses.ContainsKey(topic))
                        {
                            favoriteTopic = topic;
                            Console.WriteLine($"Chatbot: Great! I'll remember that you're interested in {topic}.");
                        }
                        else
                        {
                            Console.WriteLine($"Chatbot: Thanks for sharing, though I don't have info on '{topic}' yet.");
                        }
                        goto ContinuePrompt;
                    }

                    if (userInput.Contains("exit"))
                    {
                        Console.WriteLine("Chatbot: Goodbye! Stay safe online.");
                        PlayExitSound();
                        break;
                    }

                    if (userInput.Contains("quiz"))
                    {
                        Console.WriteLine($"Chatbot: Ready for a short cybersecurity quiz{(string.IsNullOrEmpty(favoriteTopic) ? "?" : $" on {favoriteTopic}? Let's include some questions around it!")}");
                        Quiz quiz = new Quiz(userName);
                        quiz.Start();
                        goto ContinuePrompt;
                    }

                    if (userInput.Contains("tell me more") || userInput.Contains("more details") || userInput.Contains("give me a"))
                    {
                        if (!string.IsNullOrEmpty(lastTopic) && keywordResponses.ContainsKey(lastTopic))
                        {
                            var list = keywordResponses[lastTopic];
                            Console.WriteLine($"Chatbot: {list[new Random().Next(list.Count)]}");
                        }
                        else
                        {
                            Console.WriteLine("Chatbot: Can you tell me which topic you'd like more details about?");
                        }
                        goto ContinuePrompt;
                    }

                    // ✅ Keyword-based fixed response detection
                    foreach (var entry in fixedResponses)
                    {
                        if (Regex.IsMatch(userInput, $@"\b{Regex.Escape(entry.Key)}\b"))
                        {
                            lastTopic = "";
                            Console.WriteLine($"Chatbot: {entry.Value}");
                            goto ContinuePrompt;
                        }
                    }

                    // Keyword-based multi-tip responses
                    foreach (var entry in keywordResponses)
                    {
                        if (Regex.IsMatch(userInput, $@"\b{entry.Key}\b"))
                        {
                            lastTopic = entry.Key;
                            var list = entry.Value;
                            Console.WriteLine($"Chatbot: {list[new Random().Next(list.Count)]}");
                            goto ContinuePrompt;
                        }
                    }

                    PlayErrorSound();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Chatbot: I didn't quite understand that. Could you rephrase?");
                    Thread.Sleep(1500);

                ContinuePrompt:
                    if (!string.IsNullOrEmpty(favoriteTopic) &&
                        keywordResponses.ContainsKey(favoriteTopic) &&
                        (interactionCount >= 3 || userInput.Contains(favoriteTopic)))
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"\nChatbot: As someone interested in {favoriteTopic}, here are some tips:");
                        foreach (var tip in keywordResponses[favoriteTopic])
                        {
                            Console.WriteLine($" - {tip}");
                        }
                        interactionCount = 0;
                    }

                    Console.ForegroundColor = ConsoleColor.Blue;
                    bool validInput = false;
                    while (!validInput)
                    {
                        Console.Write("\nChatbot: Would you like to ask another question? (Y/N): ");
                        string continueChat = Console.ReadLine()?.ToLower();

                        if (continueChat == "y")
                        {
                            validInput = true;
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else if (continueChat == "n")
                        {
                            validInput = true;
                            Console.WriteLine("Chatbot: Goodbye! Stay safe online.");
                            PlayExitSound();
                            return;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Thread.Sleep(1000);
                            Console.WriteLine("Chatbot: Please input 'Y' for Yes or 'N' for No.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    PlayErrorSound();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Chatbot encountered an error: {ex.Message}. Please try again.");
                }
            }
        }

        private void PlayErrorSound()
        {
            try { errorPlayer.Play(); }
            catch (Exception ex) { Console.WriteLine($"Error playing error sound: {ex.Message}"); }
        }

        private void PlayExitSound()
        {
            try { exitPlayer.PlaySync(); }
            catch (Exception ex) { Console.WriteLine($"Error playing exit sound: {ex.Message}"); }
        }
    }
}

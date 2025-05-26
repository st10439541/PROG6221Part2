using System;

namespace ProgPart2
{
    internal class Quiz
    {
        //third commit
        private string userName;

        public Quiz(string name)
        {
            userName = name;
        }

        public void Start()
        {
            bool retakeQuiz = true;

            while (retakeQuiz)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nChatbot: Great, {userName}! Let's start the quiz.");

                int score = 0;

                // Question 1
                Console.WriteLine("\n1. What makes a password strong?");
                Console.WriteLine("A. Your pet’s name\nB. '123456'\nC. A mix of letters, numbers, and symbols");
                Console.Write("Your answer: ");
                string answer1 = Console.ReadLine().Trim().ToLower();

                if (answer1 == "c") score++;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Chatbot: That answer is incorrect.");
                    Console.WriteLine("Reason: A strong password includes a combination of letters, numbers, and symbols.");
                }

                // Question 2
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n2. What is phishing?");
                Console.WriteLine("A. A way to catch fish\nB. A scam to trick people into giving personal info\nC. A virus");
                Console.Write("Your answer: ");
                string answer2 = Console.ReadLine().Trim().ToLower();

                if (answer2 == "b") score++;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Chatbot: That answer is incorrect.");
                    Console.WriteLine("Reason: Phishing is a scam that tricks you into revealing sensitive info.");
                }

                // Question 3
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n3. What does HTTPS mean?");
                Console.WriteLine("A. It’s a secure website\nB. A hacker tool\nC. Nothing important");
                Console.Write("Your answer: ");
                string answer3 = Console.ReadLine().Trim().ToLower();

                if (answer3 == "a") score++;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Chatbot: That answer is incorrect.");
                    Console.WriteLine("Reason: HTTPS means the website uses encryption to protect your data.");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nChatbot: You scored {score}/3. Well done, {userName}!");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Chatbot: Thanks for taking the quiz!");

                Console.ForegroundColor = ConsoleColor.Blue;
                bool validInput = false;

                while (!validInput)
                {
                    Console.Write("\nChatbot: Would you like to retake the quiz? (Y/N): ");
                    string retryInput = Console.ReadLine().Trim().ToLower();

                    if (retryInput == "y")
                    {
                        validInput = true;
                        Console.Clear();
                    }
                    else if (retryInput == "n")
                    {
                        validInput = true;
                        retakeQuiz = false;
                        Console.WriteLine("Chatbot: No problem! Feel free to ask another question.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Chatbot: Please enter 'Y' to retake or 'N' to exit the quiz.");
                    }
                }
            }
        }
    }
}

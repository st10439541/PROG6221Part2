using System;
using System.Media;
using System.Threading;

namespace ProgPart2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string opening = @"E:\Audio files\RachelChatBot.wav"; //for the start
            string errorSoundPath = @"E:\Audio files\errorsound.wav"; //if any error occurs

            Console.WriteLine("\nLoading chatbot...");
            Thread.Sleep(2000);
            Console.WriteLine("\nDone! Let's head in");

            try
            {
                using (SoundPlayer player = new SoundPlayer(opening))
                {
                    player.PlaySync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing sound: " + ex.Message);
            }

            string name = "";
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Please enter your name: ");
                name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    try
                    {
                        using (SoundPlayer errorPlayer = new SoundPlayer(errorSoundPath))
                        {
                            errorPlayer.PlaySync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error playing error sound: " + ex.Message);
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You didn't provide a name. Please input one.");
                    Console.ResetColor();
                }
            }

            Console.WriteLine(@"
                         (o_o) /    
                         <)   )╯    
                         //  \\     
            ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Hello {name}, welcome to the Cybersecurity Bot!");
            new Response(name);
        }
    }
}

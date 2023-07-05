using System.Text;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Channel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string logo = @"
                                        _           _            
     /\                                | |         | |           
    /  \    _____   _ _ __ ___   ___   | |     __ _| |__         
   / /\ \  |_  / | | | '_ ` _ \ / _ \  | |    / _` | '_ \        
  / ____ \  / /| |_| | | | | | | (_) | | |___| (_| | |_) |       
 /_/    \_\/___|\__,_|_| |_| |_|\___/  |______\__,_|_.__/        
  _______   _                                  ____        _     
 |__   __| | |                                |  _ \      | |    
    | | ___| | ___  __ _ _ __ __ _ _ __ ___   | |_) | ___ | |_   
    | |/ _ \ |/ _ \/ _` | '__/ _` | '_ ` _ \  |  _ < / _ \| __|  
    | |  __/ |  __/ (_| | | | (_| | | | | | |_| |_) | (_) | |_ _ 
  __|_|\___|_|\___|\__, |_|  \__,_|_| |_| |_(_)____/ \___/ \__(_)
 |  ____|           __/ |                         | |            
 | |__ _ __ __ _ _ |___/_   _____      _____  _ __| | __         
 |  __| '__/ _` | '_ ` _ \ / _ \ \ /\ / / _ \| '__| |/ /         
 | |  | | | (_| | | | | | |  __/\ V  V / (_) | |  |   <          
 |_|  |_|  \__,_|_| |_| |_|\___| \_/\_/ \___/|_|  |_|\_\         
";
                ConsoleHelper.WriteLine(logo);

                string botToken = "5226896598:AAG7jEdVOBEGQn0x-5klLFSov7W9Er4XKK0";

                ITelegramBot telegramBot = TelegramBotBuilder.Create()
                    .AddProxy("http://127.0.0.1:7890")
                    .AddToken(botToken)
                    .AddConfig(x =>
                    {

                    })
                    .AddBotName("Test")
                    .AddReceiverOptions(new ReceiverOptions { AllowedUpdates = { } })
                    .Build();

                Task task = telegramBot.BotStart();
                task.Wait();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"发生致命错误：");
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConsoleHelper
    {

        static ConsoleHelper()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public static void WriteLine(string str, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(str);
            Console.ResetColor();
        }

        public static void WriteLine(string str)
        {
            Console.ResetColor();
            Console.WriteLine(str);
        }

        public static void Write(string str, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(str);
            Console.ResetColor();
        }

        public static void Write(string str)
        {
            Console.ResetColor();
            Console.Write(str);
        }

        public static void Info(string str)
        {
            Log(str, nameof(Info), ConsoleColor.Green);
            Write($" {str}");
            Console.WriteLine();
        }

        public static void Error(string str)
        {
            Log(str, nameof(Error), ConsoleColor.Red);
            Write($" {str}", ConsoleColor.Red);
            Console.WriteLine();
        }

        public static void Warn(string str)
        {
            Log(str, nameof(Warn), ConsoleColor.DarkYellow);
            Write($" {str}");
            Console.WriteLine();
        }

        public static void Log(string str, string name, ConsoleColor consoleColor)
        {
            Write("[");
            Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}", ConsoleColor.Green);
            Write("]");
            Write("[");
            Write($"Thread:", ConsoleColor.DarkMagenta);
            Write($"{Environment.CurrentManagedThreadId}", ConsoleColor.Green);
            Write("]");
            Write("[");
            Write(name, consoleColor);
            Write("]");
        }
    }
}
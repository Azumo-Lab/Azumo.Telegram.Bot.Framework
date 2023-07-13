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
            Console.WriteLine(1 << 0);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            throw new Exception("Error");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            ConsoleHelper.Error(ex.Message);
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
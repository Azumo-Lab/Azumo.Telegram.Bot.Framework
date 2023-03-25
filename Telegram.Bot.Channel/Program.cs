using System.ComponentModel;
using Telegram.Bot.Framework.Payment.AliPay;

namespace Telegram.Bot.Channel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();

            Console.WriteLine("Hello World");

            Console.ReadLine();
        }

        private static void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Start Work");
            Thread.Sleep(5000);
            Console.WriteLine("End Work");
        }
    }
}
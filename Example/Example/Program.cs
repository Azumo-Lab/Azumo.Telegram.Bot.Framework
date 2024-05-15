// See https://aka.ms/new-console-template for more information

//var telegramBot = TelegramBot.CreateBuilder()
//    .UseClashDefaultProxy()
//    .UseToken("<Token>")
//    .Build();

//await telegramBot.StartAsync(true);

using System.Runtime.CompilerServices;

static T Instance<T>() where T : new() => new();

RuntimeHelpers.PrepareDelegate(Instance<List<string>>);

long S1;
long S2;
long B1;
long B2;

List<List<string>> list = [];
Console.WriteLine("S1 = " + (S1 = DateTime.Now.Ticks));
for (var i = 0; i < 9999999; i++)
{
    var itemList = Instance<List<string>>();
    itemList.Add(i.ToString());
    list.Add(itemList);
}
Console.WriteLine("S2 = " + (S2 = DateTime.Now.Ticks));
list.Clear();
Console.WriteLine("B1 = " + (B1 = DateTime.Now.Ticks));
for (var i = 0; i < 9999999; i++)
{
    var itemList = new List<string>
    {
        i.ToString()
    };
    list.Add(itemList);
}
Console.WriteLine("B2 = " + (B2 = DateTime.Now.Ticks));

Console.WriteLine("S = " + (S2 - S1));
Console.WriteLine("B = " + (B2 - B1));
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Controller;

namespace Telegram.Bot.Channel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {

                ITelegramBot telegramBot = TelegramBotBuilder.Create()
                    .AddToken("5226896598:AAFa9N1GiF_i7W0fV4aWgz22IGv8kzVZ13Q")
                    .AddDefaultClash()
                    .Build();

                Task botTask = telegramBot.BotStart();
                botTask.Wait();

                //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 8080);
                //socket.Bind(iPEndPoint);
                //socket.Listen();

                //ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart((obj) =>
                //{
                //    while (true)
                //    {
                //        Socket newSS = socket.Accept();
                //        ThreadPool.QueueUserWorkItem<Socket>(SocketServer, newSS, true);
                //    }
                //});
                //Semaphore semaphore = new Semaphore(20, 20);
                //semaphore.WaitOne();

                //semaphore.Release();
                //Thread Listen1 = new Thread(parameterizedThreadStart);
                //Listen1.Start();
                //Thread Listen2 = new Thread(parameterizedThreadStart);
                //Listen2.Start();

                //string Host = ArgsHelper.GetArgs(args, "-Host");
                //if (string.IsNullOrEmpty(Host))
                //    Host = "http://localhost:8080/";
                //if (!Host.EndsWith("/"))
                //    Host += "/";

                //HttpListener listener = new HttpListener();
                //listener.Prefixes.Add(Host);
                //listener.Start();

                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine($"[{DateTime.Now}] 服务器已经成功启动");
                //Console.WriteLine($"[{DateTime.Now}] 监听地址：{Host}");
                //Console.WriteLine($"[{DateTime.Now}] 请将HTML文件放在这个目录下 ：");
                //Console.WriteLine($"[{DateTime.Now}] '{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HTML")}'");
                //Console.ResetColor();

                //Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HTML"));

                //while (true)
                //{
                //    try
                //    {
                //        Task<HttpListenerContext> TaskContext = listener.GetContextAsync();
                //        Task task = TaskContext.ContinueWith(async (task) =>
                //        {
                //            HttpListenerContext httpListenerContext = await task;
                //            ThreadPool.QueueUserWorkItem<HttpListenerContext>(Context, httpListenerContext, true);
                //        });
                //        await task;
                //    }
                //    catch (Exception)
                //    {

                //    }
                //}

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"发生致命错误：");
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
            
        }

        public static async void SocketServer(Socket socket)
        {
            try
            {
                Console.WriteLine($"[{DateTime.Now}][Thread:{Environment.CurrentManagedThreadId}]");

                byte[] buffer = new byte[1024 * 1024];
                int len = socket.Receive(buffer);
                string req = Encoding.UTF8.GetString(buffer, 0, len);

                string statusline = "HTTP/1.1 200 OK\r\n";   //状态行
                byte[] statusline_to_bytes = Encoding.UTF8.GetBytes(statusline);

                byte[] Helloworld = Encoding.UTF8.GetBytes("Hello World");

                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                httpResponseMessage.Version = HttpVersion.Version11;
                httpResponseMessage.Headers.Server.Add(ProductInfoHeaderValue.Parse("AXUMO"));
                httpResponseMessage.Content = new StringContent("Hello World");

                string header = string.Format(httpResponseMessage.Headers.ToString());
                byte[] header_to_bytes = Encoding.UTF8.GetBytes(header);  //应答头
                socket.Send(statusline_to_bytes);  //发送状态行
                socket.Send(header_to_bytes);  //发送应答头
                socket.Send(new byte[] { (byte)'\r', (byte)'\n' });  //发送空行
                socket.Send(await httpResponseMessage.Content.ReadAsByteArrayAsync());  //发送正文（html）
            }
            catch (Exception)
            {

            }
            finally
            {
                socket.Close();
                socket.Dispose();
            }
        }

        public static void Context(HttpListenerContext context)
        {
            HttpListenerRequest httpListenerRequest = context.Request;

            ReadOnlySpan<byte> bytes;
            string htmlPath;
            if (File.Exists(htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HTML", string.Join(string.Empty, (httpListenerRequest.RawUrl ?? string.Empty).Skip(1)))))
            {
                bytes = new ReadOnlySpan<byte>(File.ReadAllBytes(htmlPath));
                context.Response.OutputStream.Write(bytes);
            }
            else
            {
                if ((httpListenerRequest.RawUrl ?? string.Empty) == "/")
                {
                    string[] files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HTML"), "index.*", SearchOption.TopDirectoryOnly);
                    if (files.Any())
                    {
                        bytes = new ReadOnlySpan<byte>(File.ReadAllBytes(files[0]));
                        context.Response.OutputStream.Write(bytes);
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        context.Response.OutputStream.Write(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes("404 NotFound")));
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.OutputStream.Write(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes("404 NotFound")));
                }
            }
           
            
            Console.WriteLine($"[{DateTime.Now}][Thread:{Environment.CurrentManagedThreadId}][请求方法:{httpListenerRequest.HttpMethod}][IP:{httpListenerRequest.RemoteEndPoint}][返回状态:{context.Response.StatusCode}] 请求地址：{httpListenerRequest.RawUrl}");
            context.Response.Headers.Remove(HttpResponseHeader.Server);
            context.Response.Headers.Add(HttpResponseHeader.Location, "Azumo Server V0.1");
            context.Response.Close();
        }
    }

    internal class MyHttpRequest : HttpRequest
    {
        public override HttpContext HttpContext => throw new NotImplementedException();

        public override string Method { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Scheme { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsHttps { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override HostString Host { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override PathString PathBase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override PathString Path { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override QueryString QueryString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IQueryCollection Query { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Protocol { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IHeaderDictionary Headers => throw new NotImplementedException();

        public override IRequestCookieCollection Cookies { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override long? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Stream Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool HasFormContentType => throw new NotImplementedException();

        public override IFormCollection Form { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    internal class MyHttpResponse : HttpResponse
    {
        public override HttpContext HttpContext => throw new NotImplementedException();

        public override int StatusCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IHeaderDictionary Headers => throw new NotImplementedException();

        public override Stream Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override long? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IResponseCookies Cookies => throw new NotImplementedException();

        public override bool HasStarted => throw new NotImplementedException();

        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }

        public override void OnStarting(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }

        public override void Redirect(string location, bool permanent)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 帮助创建收集参数的工具类
    /// </summary>
    public static class ArgsHelper
    {
        /// <summary>
        /// 获取参数， -Token "Token" -Proxy "http://127.0.0.1:7890/"
        /// </summary>
        /// <param name="args">参数列表</param>
        /// <param name="name">想要捕获的参数</param>
        /// <returns>返回的是想要的参数</returns>
        public static string GetArgs(this string[] args, string name)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.ToLower() == name.ToLower() || arg.ToUpper() == name.ToUpper())
                    return args.GetValue(i + 1, string.Empty);
            }
            return string.Empty;
        }

        public static T GetValue<T>(this IEnumerable<T> values, int index, T defVal)
        {
            if (IsEmpty(values) || values.Count() <= index)
                return defVal;
            return values.Skip(index).FirstOrDefault()!;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> values)
        {
            return values == null || !values.Any();
        }
    }
}
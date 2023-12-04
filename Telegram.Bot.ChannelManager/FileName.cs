namespace Telegram.Bot.ChannelManager
{
    internal class 角色
    {
        public static 角色 operator |(角色 a, string aa)
        {
            return new 角色();
        }

        public static 角色 operator |(角色 a, string[] aa)
        {
            return new 角色();
        }

        public static 角色 operator |(角色 a, IAction aa)
        {
            return new 角色();
        }

        public static void Test()
        {
            角色 角色Anzu = new();
            角色Anzu = 角色Anzu
                | "笑着活下去" | ["笑.PNG", "悲情BGM.mp3"]
                | 动作.翻转 | 等待.按秒等待(5)
                | "再见了" | ["欢乐颂.MP3"]
                | 动作.消失;
        }
    }

    public class 动作
    {
        public static IAction 翻转 { get; } = null!;

        public static IAction 消失 { get; } = null!;
    }

    public class 等待
    {
        public static IAction 按秒等待(int s)
        {
            return null!;
        }

        public static IAction 按分等待(int s)
        {
            return null!;
        }
    }

    public interface IAction
    {

    }
}

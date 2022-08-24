using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.FrameworkHelper
{
    public static class TestHelper
    {
        public static void Test()
        {
            var interfaceClass = typeof(ITestInterface);

            var bo1 = interfaceClass.IsSubclassOf(typeof(ScopedTest));
            var bo2 = interfaceClass.IsAssignableFrom(typeof(ScTest));
            var bo3 = interfaceClass.IsAssignableFrom(typeof(CC));
        }
    }

    public interface ITestInterface
    {
        void Test();
    }

    public class ScopedTest : ITestInterface
    {
        public ScopedTest(IServiceProvider serviceProvider)
        {

        }
        public Guid Guid = Guid.NewGuid();
        public void Test()
        {
            Console.WriteLine("Hello World " + Guid.ToString());
        }
    }

    public class ScTest : ITestInterface
    {
        public void Test()
        {
            Console.WriteLine("II");
        }
    }

    public class CC : ScTest
    {

    }
}

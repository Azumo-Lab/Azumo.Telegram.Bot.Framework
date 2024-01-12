using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TestProject
{
    [TestClass]
    public class UnitTest1/* : PageTest*/
    {
        //[TestMethod]
        //public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        //{
        //    await Page.GotoAsync("https://playwright.dev");

        //    // Expect a title "to contain" a substring.
        //    await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

        //    // create a locator
        //    var getStarted = Page.Locator("text=Get Started");

        //    // Expect an attribute "to be strictly equal" to the value.
        //    await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

        //    // Click the get started link.
        //    await getStarted.ClickAsync();

        //    // Expects the URL to contain intro.
        //    await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
        //}

        public static Func<IServiceProvider, object[], Task> BuildFunc(MethodInfo methodInfo, object obj)
        {
            // 获取方法的参数
            var methodParamInfos = methodInfo.GetParameters();

            // 设置参数
            var serviceProvider = Expression.Parameter(typeof(IServiceProvider), "service");
            var parameterExpression = Expression.Parameter(typeof(object[]), "args");
            var argsExpression = new Expression[methodParamInfos.Length];

            // 设置数组参数
            for (var i = 0; i < argsExpression.Length; i++)
            {
                var indexExpression = Expression.Constant(i);
                var arrayParam = Expression.ArrayIndex(parameterExpression, indexExpression);
                argsExpression[i] = Expression.Convert(arrayParam, methodParamInfos[i].ParameterType);
            }
            Expression newInvoker;
            if (obj is Type controllerType)
            {
                // 传入的是一个类型
                Expression<Func<IServiceProvider, object>> newInvokerParams = (serviceProvider) =>
                    ActivatorUtilities.CreateFactory(typeof(string), null! );
                newInvoker = Expression.Invoke(newInvokerParams, serviceProvider);
            }
            else
            {
                // 传入的是一个实例
                newInvoker = Expression.Constant(obj);
            }
            // 调用方法
            var invoker = Expression.Call(Expression.Convert(newInvoker, methodInfo.DeclaringType!), methodInfo, argsExpression);
            Expression<Func<object, Task>> result = (obj) => obj as Task ?? Task.CompletedTask;
            var task = methodInfo.ReturnType != typeof(void)
                ? Expression.Invoke(result, invoker)
                : (Expression)Expression.Block(invoker, Expression.Invoke(result, Expression.Constant(new object(), typeof(object))));
            var express = Expression.Lambda<Func<IServiceProvider, object[], Task>>(task, serviceProvider, parameterExpression);
            return express.Compile();
        }

        [TestMethod]
        public void Test()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<TestBB>()
                .AddTransient<TestCC>()
                .BuildServiceProvider();

            var objectFactory = ActivatorUtilities.CreateFactory<Test>([]);
            for (var i = 0; i < 1000000; i++)
            {
                var obj = objectFactory(serviceProvider, []);
                obj.TestInvoke("Hello");
            }
            Console.WriteLine();
        }
    }

    public class Test
    {
        public Test(TestBB testBB)
        {

        }

        public void TestInvoke(string str)
        {
            Console.WriteLine("Hello World " + str);
            //return Task.CompletedTask;
        }
    }

    public class TestBB
    {
        public TestBB(TestCC testCC)
        {

        }
    }

    public class TestCC
    {

    }
}

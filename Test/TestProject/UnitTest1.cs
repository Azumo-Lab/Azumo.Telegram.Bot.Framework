using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TestProject;

[TestClass]
public class UnitTest1/* : PageTest*/
{
    private static readonly object[] arguments = [];

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

    public static Func<IServiceProvider, object[], Task> BuildFunc(MethodInfo methodInfo, ObjectFactory obj)
    {
        // ��ȡ�����Ĳ���
        var methodParamInfos = methodInfo.GetParameters();

        // ���ò���
        var serviceProvider = Expression.Parameter(typeof(IServiceProvider), "service");
        var parameterExpression = Expression.Parameter(typeof(object[]), "args");
        var argsExpression = new Expression[methodParamInfos.Length];

        // �����������
        for (var i = 0; i < argsExpression.Length; i++)
        {
            var indexExpression = Expression.Constant(i);
            var arrayParam = Expression.ArrayIndex(parameterExpression, indexExpression);
            argsExpression[i] = Expression.Convert(arrayParam, methodParamInfos[i].ParameterType);
        }
        Expression newInvoker = Expression.Invoke((IServiceProvider service) => obj(service, arguments), serviceProvider);

        // ���÷���
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

        var func = BuildFunc(typeof(Test).GetMethods().First(), ActivatorUtilities.CreateFactory(typeof(Test), []));
        RuntimeHelpers.PrepareDelegate(func);

        for (var i = 0; i < 1000000; i++)
        {
            _ = func(serviceProvider, ["Hello"]);
        }

        for (var i = 0; i < 1000000; i++)
        {
            new Test(new TestBB(new TestCC())).TestInvoke("Hello");
        }

        Console.WriteLine();
    }
}

public class Test(TestBB testBB)
{
    public void TestInvoke(string str) => 
        Console.WriteLine("Hello World " + str + testBB.testCC1.Hello);//return Task.CompletedTask;
}

public class TestBB(TestCC testCC)
{
    public readonly TestCC testCC1 = testCC;
}

public class TestCC
{
    public string Hello = "Hello";
}

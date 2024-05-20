//var telegramBot = TelegramBot.CreateBuilder()
//    .UseClashDefaultProxy()
//    .UseToken("<Token>")
//    .Build();

//await telegramBot.StartAsync(true);

using Example;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

internal class Program
{
    internal static async Task ConvertTo(Task task) =>
        await task;

    private static async Task Main(string[] args)
    {
        var methodinfo = typeof(Program).GetMethod("TaskStart", BindingFlags.Static | BindingFlags.Public)!;

        var returnType = methodinfo.ReturnType;

        var list = new List<Expression>();

        Expression CreateExpression(Type type)
        {
            // Task<Task<string>>
            // 把 Task 类型 转换为 Task<Task<string>>，并调用Result
            var TaskAwait = Expression.Parameter(typeof(Task), "TaskAwait");
            var TaskConvert = Expression.Convert(TaskAwait, type);
            // Task<string>
            var result = Expression.Property(TaskConvert, nameof(Task<object>.Result));
            Expression resuFunc = Expression.Lambda<Func<Task, object?>>(result, TaskAwait);
            return resuFunc;
        }

        var methodReturnInstance = Expression.Parameter(returnType);
        var convertTo = typeof(Program).GetMethod(nameof(ConvertTo), BindingFlags.Static | BindingFlags.NonPublic)!;
        
        // 呼叫ConvertTo方法
        var taskMethodResult = Expression.Call(null, convertTo, methodReturnInstance);
        Expression convertValue = Expression.Invoke(CreateExpression(returnType), methodReturnInstance);
        // 呼叫ConvertTo方法
        var tt = Expression.Block(taskMethodResult, convertValue);

        list.Add(tt);

        var childType = returnType.GetGenericArguments();
        var expression = convertValue;
        NEXT:
        foreach (var item in childType)// Task<string>
        {
            // 泛型依然是Task<>类型
            if (item.IsGenericType && item.GetGenericTypeDefinition() == typeof(Task<>))
            {
                // 将Object类型转换为 内部的 泛型
                var taskValue = Expression.Convert(expression, item);
                // 呼叫ConvertTo方法，并将新参数传入
                var aa = Expression.Call(null, convertTo, taskValue);
                // 返回新的泛型类型
                expression = Expression.Invoke(CreateExpression(item), taskValue);
                var cc = Expression.Block(aa, taskValue);

                list.Add(cc);
                childType = item.GetGenericArguments();
                goto NEXT;
            }
        }

        list.Add(expression);

        var ttee = Expression.Lambda(Expression.Block(list), methodReturnInstance).Compile();
        RuntimeHelpers.PrepareDelegate(ttee);

    }

    public static async Task<Task<Task<Task<Task<Task<Task<string>>>>>>> TaskStart() =>
        await Task.FromResult(Task.FromResult(Task.FromResult(Task.FromResult(Task.FromResult(Task.FromResult(Task.FromResult("123")))))));
}
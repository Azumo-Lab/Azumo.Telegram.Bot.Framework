using Azumo.PipelineMiddleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.PipelineTest.TestModels;

namespace TestProject.PipelineTest;

[TestClass]
public class PipelineTestClass
{
    private const string Invoke = "Invoke";
    [TestMethod]
    public void Test001()
    {
        var builder = PipelineFactory.GetPipelineBuilder<TestModel001>();
        var controller = builder
            .NewPipeline(Invoke)
            .Use(new Test00(), MiddlewareInsertionMode.StartOfPhase)
            .Use(new Test01(), MiddlewareInsertionMode.EndOfPhase)
            .Use(new Test02(), MiddlewareInsertionMode.EndOfPhase)
            .Use(new Test03(), MiddlewareInsertionMode.EndOfPhase)
            .Use(new Test04(), MiddlewareInsertionMode.EndOfPhase)
            .Use(new Test05(), MiddlewareInsertionMode.EndOfPhase)
            .Use(new Test06(), MiddlewareInsertionMode.EndOfPhase)
            .Use(new TestInvokeFilter())
            .Build();

        var test = new TestModel001
        {
            Str = "dghjiudghuir"
        };

        var pipeline = controller.GetPipeline(Invoke);
        pipeline.Invoke(test);

        controller.Execute(Invoke, test);
    }
}

public class Test00 : IMiddleware<TestModel001>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.PreProcessing;

    public string MiddlewareName => "保存 备份";

    public Task Execute(TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        input.Copy = input.Str;

        return pipelineController.Next(input);
    }
}

public class Test01 : IMiddleware<TestModel001>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.PreProcessing;

    public string MiddlewareName => "字母清洗";

    public Task Execute(TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        input.Str = input.Str!.Replace("h", " ");
        input.Str = input.Str!.Replace("g", " ");
        input.Str = input.Str!.Replace("u", " ");

        return pipelineController.Next(input);
    }
}

public class Test02 : IMiddleware<TestModel001>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

    public string MiddlewareName => "字母替换 => [d]";

    public Task Execute(TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        input.Str = input.Str!.Replace("d", "[D]");

        return pipelineController.Next(input);
    }
}

public class Test03 : IMiddleware<TestModel001>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

    public string MiddlewareName => "字母替换 => [i]";

    public Task Execute(TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        input.Str = input.Str!.Replace("i", "[i]");

        return pipelineController.Next(input);
    }
}

public class Test04 : IMiddleware<TestModel001>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.PostProcessing;

    public string MiddlewareName => "句尾后处理";

    public Task Execute(TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        input.Str += "[EOL]";

        return pipelineController.Next(input);
    }
}

public class Test05 : IMiddleware<TestModel001>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.PostProcessing;

    public string MiddlewareName => "换行后处理";

    public Task Execute(TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        input.Str += "\\n";

        return pipelineController.Next(input);
    }
}

public class Test06 : IMiddleware<TestModel001>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.PreProcessing;

    public string MiddlewareName => "字母清理：g";

    public Task Execute(TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        input.Str = input.Str!.Replace("g", " ");

        return pipelineController.Next(input);
    }
}
public class TestInvokeFilter : IPipelineInvokeFilter<TestModel001>
{
    public bool Filter(Delegate handle, IMiddleware<TestModel001> middleware, TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        var name = (middleware as IMiddlewareName)?.MiddlewareName ?? "Unknow";
        input.InvokeLink.Add(name);
        return true;
    }
}
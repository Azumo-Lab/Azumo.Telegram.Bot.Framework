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
    [TestMethod]
    public void Test001()
    {
        var builder = PipelineFactory.GetPipelineBuilder<TestModel001>();
        var controller = builder
            .NewPipeline("Str")
            .Use(new Test())
            .Use(new Test00())
            .Use(new TestInvokeFilter())
            .NewPipeline("Test")
            .Use(new Test01())
            .Build();

        var test = new TestModel001();

        controller.Execute("Str", test);
    }
}

public class Test : IMiddleware<TestModel001>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.PostProcessing;

    public string MiddlewareName => "Say Hello";

    public Task Execute(TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        input.Str = "Hello";

        return pipelineController.Next(input);
    }
}

public class Test00 : IMiddleware<TestModel001>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

    public string MiddlewareName => "Say World";

    public Task Execute(TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        input.Str += "World";

        return pipelineController.Execute("Test", input);
    }
}

public class Test01 : IMiddleware<TestModel001>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

    public string MiddlewareName => "Clear";

    public Task Execute(TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        input.Str = "";

        return pipelineController.Next(input);
    }
}
public class TestInvokeFilter : IPipelineInvokeFilter<TestModel001>
{
    public bool Filter(Delegate handle, IMiddleware<TestModel001> middleware, TestModel001 input, IPipelineController<TestModel001> pipelineController)
    {
        var name = (middleware as IMiddlewareName)?.MiddlewareName ?? "Unknow";
        input.InvokeLink += name + ",";
        return true;
    }
}
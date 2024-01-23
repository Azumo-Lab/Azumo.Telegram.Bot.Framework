using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.PipelineTest.TestModels;
public class TestModel001
{
    public string? Str { get; set; }

    public string? Copy { get; set; }

    public List<string> InvokeLink { get; set; } = [];
}

using Pipeline.Framework;
using Pipeline.Framework.Abstracts;
using System.Text;

namespace AOT
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            IPipelineBuilder<(string, string)> builder = InternalFactory.CreateIPipelineBuilder<(string, string)>();
            IPipelineController<(string, string)> pipelineController =
                 builder.AddProcedure(new 加工产品())
                        .AddProcedure(new 检测不良品())
                        .AddProcedure(new 包装产品())
                        .AddProcedure(new 产品出厂())
                        .CreatePipeline("合格产品")

                        .AddProcedure(new 修复不良品())
                        .AddProcedure(new 包装产品())
                        .AddProcedure(new 产品出厂())
                        .CreatePipeline("可修复产品")

                        .AddProcedure(new 废弃不良品())
                        .CreatePipeline("不可修复产品")
                        .BuilderPipelineController();

            while (true)
            {
                pipelineController.ChangePipeline("合格产品");

                Console.WriteLine("请输入产品：");
                string input = Console.ReadLine() ?? string.Empty;
                (string, string) processStr = pipelineController.Next((string.Empty, input)).Result;

                Console.WriteLine("最终结果：");
                Console.WriteLine(processStr.Item1);
                Console.WriteLine(processStr.Item2);
            }
        }
    }

    public class 包装产品 : IProcedure<(string, string)>
    {
        public async Task<(string, string)> Execute((string, string) t, IPipelineController<(string, string)> pipelineController)
        {
            t.Item1 += " 包装 ";
            t.Item2 = $"[{t.Item2}]";
            Console.WriteLine("包装好了");
            return await pipelineController.Next(t);
        }
    }

    public class 检测不良品 : IProcedure<(string, string)>
    {
        public async Task<(string, string)> Execute((string, string) t, IPipelineController<(string, string)> pipelineController)
        {
            t.Item1 += " 检测不良品 ";
            if (t.Item2.EndsWith('!'))
            {
                Console.WriteLine("检测到说话带叹号的不良品，尝试修复");
                pipelineController.ChangePipeline("可修复产品");
                return await pipelineController.Next(t);
            }
            else
            {
                Console.WriteLine("说话不带叹号，合格");
                return await pipelineController.Next(t);
            }
        }
    }

    public class 修复不良品 : IProcedure<(string, string)>
    {
        public async Task<(string, string)> Execute((string, string) t, IPipelineController<(string, string)> pipelineController)
        {
            t.Item1 += " 修复不良品 ";
            t.Item2 = t.Item2[..^1];
            if (t.Item2.EndsWith('!'))
            {
                Console.WriteLine("依然带叹号，属于不可修复产品，修复失败");
                pipelineController.ChangePipeline("不可修复产品");
                return await pipelineController.Next(t);
            }
            else
            {
                Console.WriteLine("修复成功");
                return await pipelineController.Next(t);
            }
        }
    }

    public class 加工产品 : IProcedure<(string, string)>
    {
        public async Task<(string, string)> Execute((string, string) t, IPipelineController<(string, string)> pipelineController)
        {
            t.Item1 += " 加工产品 ";
            t.Item2 = t.Item2.TrimStart().TrimEnd();

            Console.WriteLine("加工产品，去掉前后空格");

            return await pipelineController.Next(t);
        }
    }

    public class 产品出厂 : IProcedure<(string, string)>
    {
        public async Task<(string, string)> Execute((string, string) t, IPipelineController<(string, string)> pipelineController)
        {
            t.Item1 += " 产品出厂 ";

            Console.WriteLine("产品出厂");
            Console.WriteLine($"产品经过过了 ： 【{t.Item1}】 工序");
            Console.WriteLine($"最后的产品 : {t.Item2}");

            return await pipelineController.Next(t);
        }
    }

    public class 废弃不良品 : IProcedure<(string, string)>
    {
        public async Task<(string, string)> Execute((string, string) t, IPipelineController<(string, string)> pipelineController)
        {
            t.Item1 += " 废弃不良品 ";
            Console.WriteLine("产品废弃");
            return await pipelineController.Stop((t.Item1, ""));
        }
    }
}

//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Abstract.Config;
using Telegram.Bot.Framework.Abstracts.Bot;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 框架配置
    /// 
    /// Expression表达式 微软文档
    /// https://learn.microsoft.com/zh-cn/dotnet/csharp/advanced-topics/expression-trees/expression-trees-interpreting
    /// Expression表达式的一个博客文章
    /// https://www.cnblogs.com/li-peng/p/3154381.html
    /// 
    /// 一个2D物理方面的项目，可以制作游戏
    /// https://github.com/Genbox/VelcroPhysics
    /// SkiaSharp 之 WPF 自绘 五环弹动球（案例版）
    /// https://www.cnblogs.com/kesshei/p/16538440.html
    /// SkiaSharp 之 WPF 自绘 粒子花园（案例版）
    /// https://www.cnblogs.com/kesshei/p/16548913.html
    /// SkiaSharp 之 WPF 自绘 投篮小游戏（案例版）
    /// https://www.cnblogs.com/kesshei/p/16552463.html
    /// Skia的文档
    /// https://skia.org/docs/user/api/skcanvas_overview/
    /// SkiaSharp的微软文档
    /// https://learn.microsoft.com/zh-cn/dotnet/api/skiasharp.skcanvas?view=skiasharp-2.88
    /// SkiaSharp项目i，可以用来制作游戏
    /// https://github.com/mono/SkiaSharp
    /// A-Star（A*）寻路算法原理与实现
    /// https://zhuanlan.zhihu.com/p/385733813
    /// 大佬的WPF游戏制作博文系列
    /// https://www.cnblogs.com/alamiye010/archive/2009/06/17/1505332.html
    /// https://www.cnblogs.com/alamiye010/archive/2009/06/17/1505346.html
    /// 
    /// 一个博客，主要内容是WPF，C#这一类的内容
    /// https://blog.lindexi.com/
    /// 
    /// C#中使用Socket实现简单Web服务器
    /// https://www.cnblogs.com/xiaozhi_5638/p/3917943.html
    /// 
    /// 火狐的文档，关于HTTP的一些
    /// https://developer.mozilla.org/zh-CN/docs/Web/HTTP/Compression
    /// 
    /// 用 C# 自己动手编写一个 Web 服务器，第二部分——中间件
    /// https://shuhari.dev/blog/2017/12/build-web-server-middleware/
    /// 
    /// 支付宝当面付
    /// https://www.cnblogs.com/stulzq/p/7647948.html
    /// 
    /// C# ConcurrentBag的实现原理的一个博客文章
    /// https://www.cnblogs.com/incerry/p/9497729.html#2-%E7%94%A8%E4%BA%8E%E6%95%B0%E6%8D%AE%E5%AD%98%E5%82%A8%E7%9A%84threadlocallist%E7%B1%BB
    /// 
    /// 写一个简单数据库的博客文章
    /// https://www.cnblogs.com/kesshei/p/16519862.html
    /// </summary>
    internal class TGConf : IStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            

            #region 中间件流水线相关的处理
            // 添加中间件流水线
            services.AddMiddlewarePipeline();
            services.AddMiddlewareTemplate();
            #endregion
        }
    }
}

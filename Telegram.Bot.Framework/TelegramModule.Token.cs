//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System;
using System.Net.Http;
using Telegram.Bot.Framework.BotBuilder;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramToken : ITelegramModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public TelegramToken(string token) => Token = token;

        /// <summary>
        /// 
        /// </summary>
        protected string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public virtual void AddBuildService(IServiceCollection services)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builderService"></param>
        /// <exception cref="NullReferenceException"></exception>
        public virtual void Build(IServiceCollection services, IServiceProvider builderService)
        {
            if (string.IsNullOrEmpty(Token))
                throw new NullReferenceException(nameof(Token));

            HttpClient? client;
            var botClient = (client = builderService.GetService<HttpClient>()) != null
                ? new TelegramBotClient(Token, client)
                : new TelegramBotClient(Token);

            _ = services.AddSingleton<ITelegramBotClient>(botClient);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="SettingModel"></typeparam>
    internal class TelegramToken<SettingModel> : TelegramToken where SettingModel : class
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Func<SettingModel, string> tokenFunc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenFunc"></param>
        public TelegramToken(Func<SettingModel, string> tokenFunc) : base(string.Empty) =>
            this.tokenFunc = tokenFunc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builderService"></param>
        /// <exception cref="NullReferenceException"></exception>
        public override void Build(IServiceCollection services, IServiceProvider builderService)
        {
            var settingModel = builderService.GetService<SettingModel>();
            if (settingModel == null)
                throw new NullReferenceException(nameof(settingModel));

            Token = tokenFunc(settingModel);

            base.Build(services, builderService);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static partial class TelegramModuleExtensions
    {
        /// <summary>
        /// 使用指定的 <see cref="Func{T, TResult}"/> 方法，来生成指定类型: <typeparamref name="SettingModel"/> 的数据
        /// </summary>
        /// <remarks>
        /// 这个配置，需要提前进行配置文件的配置操作，这里的数据类型：<typeparamref name="SettingModel"/> 要和配置文件中的配置类型相同。
        /// 可以使用以下形式的代码进行Token的配置操作。
        /// <code>
        /// x => x.Token 
        /// </code>
        /// 返回Token，请注意：<paramref name="tokenFunc"/> 返回的Token值不能为空值，否则会抛出异常 : <see cref="NullReferenceException"/>
        /// </remarks>
        /// <typeparam name="SettingModel">指定的配置类型</typeparam>
        /// <param name="builder">Telegram模块创建器</param>
        /// <param name="tokenFunc">生成Token的方法</param>
        /// <returns><see cref="ITelegramModuleBuilder"/>类型，配置完成的模块创建器</returns>
        public static ITelegramModuleBuilder UseToken<SettingModel>(this ITelegramModuleBuilder builder, Func<SettingModel, string> tokenFunc) where SettingModel : class =>
            builder.AddModule(new TelegramToken<SettingModel>(tokenFunc));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ITelegramModuleBuilder UseToken(this ITelegramModuleBuilder builder, string token) =>
            builder.AddModule(new TelegramToken(token));
    }
}

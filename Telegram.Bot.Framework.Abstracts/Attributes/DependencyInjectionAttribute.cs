//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependencyInjectionAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type? ServiceType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServiceLifetime"></param>
        /// <param name="ServiceType"></param>
        public DependencyInjectionAttribute(ServiceLifetime ServiceLifetime, Type ServiceType)
        {
            this.ServiceLifetime = ServiceLifetime;
            this.ServiceType = ServiceType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServiceLifetime"></param>
        public DependencyInjectionAttribute(ServiceLifetime ServiceLifetime)
        {
            this.ServiceLifetime = ServiceLifetime;
        }
    }
}

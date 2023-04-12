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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Telegram.Bot.Channel.DITest
{
    public class Container
    {
        private string Guid = "[Guid(\"C6608E16-C627-4123-962D-F895235513C6\")]";

        private readonly Dictionary<Type, Delegate> NewInvoke = new ();
        public static string GetInstance()
        {
            Type type = typeof(Container);
            Expression newInvoke = Expression.New(type.GetConstructors().First());
            Func<Container> NewObj = Expression.Lambda<Func<Container>>(newInvoke).Compile();

            ParameterExpression param1 = Expression.Parameter(type);
            Expression method = Expression.Field(param1, type.GetField("Guid", BindingFlags.NonPublic | BindingFlags.Instance)!);
            Func<Container, string> GetGuid = Expression.Lambda<Func<Container, string>>(method, param1).Compile();

            return GetGuid(NewObj());
        }
    }

}

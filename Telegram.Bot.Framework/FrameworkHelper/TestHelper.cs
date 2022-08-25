//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Sokushu> see <https://github.com/sokushu/Telegram.Bot.Net/>
//
//  This program is free software: you can redistribute it and/or modify
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
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.FrameworkHelper
{
    public static class TestHelper
    {
        public static void Test()
        {
            var interfaceClass = typeof(ITestInterface);

            var bo1 = interfaceClass.IsSubclassOf(typeof(ScopedTest));
            var bo2 = interfaceClass.IsAssignableFrom(typeof(ScTest));
            var bo3 = interfaceClass.IsAssignableFrom(typeof(CC));
        }
    }

    public interface ITestInterface
    {
        void Test();
    }

    public class ScopedTest : ITestInterface
    {
        public ScopedTest(IServiceProvider serviceProvider)
        {

        }
        public Guid Guid = Guid.NewGuid();
        public void Test()
        {
            Console.WriteLine("Hello World " + Guid.ToString());
        }
    }

    public class ScTest : ITestInterface
    {
        public void Test()
        {
            Console.WriteLine("II");
        }
    }

    public class CC : ScTest
    {

    }
}

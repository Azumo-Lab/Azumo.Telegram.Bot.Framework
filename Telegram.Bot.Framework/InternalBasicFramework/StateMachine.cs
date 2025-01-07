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
//
//  Author: 牛奶

namespace Telegram.Bot.Framework.InternalBasicFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class StateMachine<T> : IStateMachine<T>
    {
        /// <summary>
        /// 
        /// </summary>
        protected IStatusChangeNotification<T>? Notification { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract T State { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract void EnterState();

        /// <summary>
        /// 
        /// </summary>
        public abstract void ExitState();

        /// <summary>
        /// 
        /// </summary>
        public abstract void NextState();
    }

    internal abstract class StateMachine<T, R> : IStateMachine<T, R>
    {
        public abstract T State { get; }

        /// <summary>
        /// 
        /// </summary>
        protected IStatusChangeNotification<T>? Notification { get; set; }

        public abstract R EnterState();
        public abstract R ExitState();
        public abstract R NextState();
    }
}

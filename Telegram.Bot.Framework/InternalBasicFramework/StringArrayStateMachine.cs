//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

using System;

namespace Telegram.Bot.Framework.InternalBasicFramework
{
    /// <summary>
    /// 
    /// </summary>
    internal class StringArrayStateMachine : StateMachine<string>
    {
        /// <summary>
        /// 
        /// </summary>
        private const string DefaultState = "";

        /// <summary>
        /// 
        /// </summary>
        private const int DefaultStateIndex = -1;

        /// <summary>
        /// 
        /// </summary>
        private readonly string[] _states;

        /// <summary>
        /// 
        /// </summary>
        private int _currentStateIndex = DefaultStateIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="states"></param>
        /// <param name="statusChangeNotification"></param>
        /// <exception cref="ArgumentException"></exception>
        public StringArrayStateMachine(string[] states, IStatusChangeNotification<string>? statusChangeNotification = null)
        {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(states, nameof(states));
#else
            if (states is null)
                throw new ArgumentNullException(nameof(states));
#endif

            if (states.Length != 0)
            {
                foreach (var item in states)
                    if (string.IsNullOrEmpty(item))
                        throw new ArgumentException("State must not be null or empty");
            }
            else throw new ArgumentException("States must not be empty");
            _states = states;

            Notification = statusChangeNotification;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string State =>
            _currentStateIndex == DefaultStateIndex ? DefaultState : _states[_currentStateIndex];

        /// <summary>
        /// 
        /// </summary>
        public override void EnterState()
        {
            _currentStateIndex++;
            Notification?.OnStateChange(DefaultState, State);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ExitState()
        {
            Notification?.OnStateChange(State, DefaultState);
            _currentStateIndex = DefaultStateIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void NextState()
        {
            var oldState = State;

            _currentStateIndex++;
            if (_currentStateIndex >= _states.Length)
                _currentStateIndex = 0;

            Notification?.OnStateChange(oldState, State);
        }
    }
}

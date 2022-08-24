using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework.FrameworkHelper;

namespace Telegram.Bot.Framework.DependencyInjection
{
    public class TelegramServiceCollection : IServiceCollection, IServiceProviderBuild
    {
        private readonly List<TelegramServiceDescriptor> telegramServiceDescriptors = new List<TelegramServiceDescriptor>();
        public TelegramServiceDescriptor this[int index] { get => telegramServiceDescriptors[index]; set => telegramServiceDescriptors[index] = value; }

        public int Count => telegramServiceDescriptors.Count;

        public bool IsReadOnly => false;

        public void Add(TelegramServiceDescriptor item)
        {
            ThrowHelper.ThrowIfNull(item);

            if (Contains(item))
                return;
            telegramServiceDescriptors.Add(item);
        }

        public IServiceProvider Build()
        {
            foreach (TelegramServiceDescriptor item in telegramServiceDescriptors)
            {
                
            }
            return default;
        }

        public void Clear()
        {
            telegramServiceDescriptors.Clear();
        }

        public bool Contains(TelegramServiceDescriptor item)
        {
            return telegramServiceDescriptors.Contains(item);
        }

        public void CopyTo(TelegramServiceDescriptor[] array, int arrayIndex)
        {
            telegramServiceDescriptors.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TelegramServiceDescriptor> GetEnumerator()
        {
            return telegramServiceDescriptors.GetEnumerator();
        }

        public int IndexOf(TelegramServiceDescriptor item)
        {
            return telegramServiceDescriptors.IndexOf(item);
        }

        public void Insert(int index, TelegramServiceDescriptor item)
        {
            ThrowHelper.ThrowIfNull(item);

            if (Contains(item))
                return;
            telegramServiceDescriptors.Insert(index, item);
        }

        public bool Remove(TelegramServiceDescriptor item)
        {
            return telegramServiceDescriptors.Remove(item);
        }

        public void RemoveAt(int index)
        {
            telegramServiceDescriptors.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return telegramServiceDescriptors.GetEnumerator();
        }
    }
}

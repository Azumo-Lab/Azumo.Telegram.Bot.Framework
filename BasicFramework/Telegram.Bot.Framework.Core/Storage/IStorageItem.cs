using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Core.Storage;
public interface IStorageItem
{
    public string FullPath { get; }

    public string FileName { get; }

    public Stream GetStream();

    public void Write(byte[] bytes);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Storage;
public interface IPrivateStorage
{
    public User UserInfo { get; }

    public bool Create();

    public bool Create(IStorageItem storageItem);

    public bool Delete();

    public bool Delete(IStorageItem storageItem);

    public List<IStorageItem> GetAll();
}

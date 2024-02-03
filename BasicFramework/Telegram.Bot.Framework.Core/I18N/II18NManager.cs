using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Core.I18N;
public interface II18NManager
{
    public const string Chinese = "Chinses";

    public II18N Current { get; }

    public void Add(II18N i18N);

    public void Change(string name);

    public void Remove(string name);
}

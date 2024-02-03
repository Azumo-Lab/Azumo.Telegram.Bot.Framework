using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Core.I18N;
public interface II18N
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string this[string key] { get; }

    public void Load();
}

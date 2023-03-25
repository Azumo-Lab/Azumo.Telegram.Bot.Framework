using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.Channel.Models
{
    public sealed class Photo
    {
        public string Context { get; } = default!;

        public byte[] PhotoBytes { get; } = default!;

        public Photo(string Context, string FilePath, List<string> Tags) : this(Context, System.IO.File.OpenRead(FilePath), Tags)
        {
            
        }

        public Photo(string Context, Stream File, List<string> Tags)
        {
            StringBuilder tags = new StringBuilder();
            foreach (string tag in Tags)
            {
                string hashTag = string.Empty;
                if (!tag.StartsWith("#"))
                    hashTag = $"#{tag}";
                else
                    hashTag = tag;
                tags.Append(hashTag).Append(" ");
            }
            this.Context = new StringBuilder()
                .Append(Context)
                .AppendLine(tags.ToString())
                .ToString();

            using (MemoryStream ms = new MemoryStream())
            {
                File.CopyTo(ms);
                PhotoBytes = ms.ToArray();
            }
        }
    }
}

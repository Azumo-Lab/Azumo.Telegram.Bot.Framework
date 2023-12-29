using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Mushi
{
    public abstract class DataContext
    {
        public List<string> StartURL { get; } = [];

        public List<string> WaitingForProcessingURL { get; } = [];

        public Dictionary<object, object> Data { get; } = [];

        public List<HtmlDocument> Document { get; } = [];
    }
}

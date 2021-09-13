using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MscrmTools.WebresourcesManager.AutoResponder
{
    public class ProxyResource
    {
        public string Server { get; set; }
        public string FilePath { get; set; }
        public string UrlPath { get; set; }
        public bool isEnabled { get; set; }
    }
}

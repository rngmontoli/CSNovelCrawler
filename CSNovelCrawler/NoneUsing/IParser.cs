using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSNovelCrawler
{
    public interface IParser
    {
        
        bool CheckUrl(string Url);
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSNovelCrawler.Plugin
{
    [Serializable]
    public abstract class abstractPlugin : IPlugin
    {

        public abstract IDownloader CreateDownloader();

        public abstract bool CheckUrl(string url);

        public abstract string GetHash(string url);

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            
        }
    }
}

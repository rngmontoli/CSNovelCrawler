using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSNovelCrawler.Core
{
    [Serializable]
    public class CustomSettings
    {
        public string DefaultSaveFolder = System.Environment.CurrentDirectory;

        public bool HideSysTray = true;


        [XmlArray("Folders")]
        [XmlArrayItem("Folder")]
        public List<string> SaveFolders = new List<string>();
    }
}

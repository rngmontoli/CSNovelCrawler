using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CSNovelCrawler.Core
{
    [Serializable]
    public class CustomSettings
    {
        public string DefaultSaveFolder = Environment.CurrentDirectory;
        public bool WatchClipboard = true;
        public bool HideSysTray = true;
        public bool Logging = false;
        public string TextEncoding = Encoding.Unicode.BodyName;
        public int SubscribeTime =10;

        [XmlArray("Folders")]
        [XmlArrayItem("Folder")]
        public List<string> SaveFolders = new List<string>();
    }
}

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
        public bool HideSysTray = false;
        public bool Logging = false;
        public string TextEncoding = Encoding.UTF8.BodyName;
        public int SubscribeTime =10;


        [XmlArray("Folders")]
        [XmlArrayItem("Folder")]
        public List<string> SaveFolders = new List<string>();


        public string SelectFormatName = "書名";
        public string SelectFormat = "%Title%";
        [XmlIgnore]
        public List<FormatFileName_Class> DefaultFormatFileName
        {
            get
            {
                List<FormatFileName_Class> Formats = new List<FormatFileName_Class>();
                Formats.Add(new FormatFileName_Class() { Key = 1, Name = "書名", Format = "%Title%" });
                Formats.Add(new FormatFileName_Class() { Key = 2, Name = "作者─書名", Format = "%Author%─%Title%" });
                Formats.Add(new FormatFileName_Class() {Key=3, Name = "書名─作者", Format = "%Title%─%Author%" });
                return Formats;
            }
        }

        [XmlArray("CustomFormatFileName")]
        [XmlArrayItem("CustomFormatFileName")]
        public List<FormatFileName_Class> CustomFormatFileName = new List<FormatFileName_Class>();

        public class FormatFileName_Class
        {
            public int Key { get; set; }
            public string Name { get; set; }
            public string Format { get; set; }
        }
    }
}

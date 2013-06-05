using System;
using System.IO;
using System.Xml.Serialization;

namespace CSNovelCrawler.Core
{
    public class ConfigManager
    {
        private string _ConfigFolderPath { get{ return CoreManager.StartupPath; }}
        private string _ConfigFileName{ get{ return "config.xml";} }
        private string ConfigFullFileName { get { return Path.Combine(_ConfigFolderPath, _ConfigFileName); } }


        public CustomSettings Settings { get; set; }



        /// <summary>
        /// 儲存設定
        /// </summary>
        public void SaveSettings()
        {

            
            using (FileStream oFileStream = new FileStream(ConfigFullFileName, FileMode.Create))
            {
                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(CustomSettings));
                oXmlSerializer.Serialize(oFileStream, CoreManager.ConfigManager.Settings);
                oFileStream.Close();
            }
        }

        /// <summary>
        /// 讀取設定
        /// </summary>
        /// <returns></returns>
        public void LoadSettings()
        {
            try
            {
                CustomSettings tempSettings=null;
                if (File.Exists(ConfigFullFileName))
                {
                    
                    using (FileStream oFileStream = new FileStream(ConfigFullFileName, FileMode.Open))
                    {
                        XmlSerializer oXmlSerializer = new XmlSerializer(typeof(CustomSettings));
                        tempSettings = (CustomSettings)oXmlSerializer.Deserialize(oFileStream);
                    }
                }

                if (tempSettings != null)
                    CoreManager.ConfigManager.Settings = tempSettings;
                else
                    throw new Exception();
            }
            catch(Exception ex)
            {
                CoreManager.LoggingManager.Debug(ex.ToString());
                CoreManager.ConfigManager.Settings = new CustomSettings();
                SaveSettings();
            }
        }

    }
}

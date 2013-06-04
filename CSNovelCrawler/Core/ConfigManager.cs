using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSNovelCrawler.Core
{
    public class ConfigManager
    {
        private string _ConfigFolderPath { get{ return CoreManager.StartupPath; }}
        private string _ConfigFileName{ get{ return "config.xml";} }
        private string _ConfigFullFileName { get { return Path.Combine(_ConfigFolderPath, _ConfigFileName); } }


        public ConfigManager()
        {
            //_ConfigFolderPath = CoreManager.StartupPath;
        }

        public CustomSettings Settings { get; set; }



        /// <summary>
        /// 儲存設定
        /// </summary>
        public void SaveSettings()
        {
            try
            { //如果文件存在
            
                using (FileStream oFileStream = new FileStream(_ConfigFullFileName, FileMode.Create))
                {
                    XmlSerializer oXmlSerializer = new XmlSerializer(typeof(CustomSettings));
                    oXmlSerializer.Serialize(oFileStream, CoreManager.ConfigManager.Settings);
                    oFileStream.Close();
                }
            
            }
            catch (Exception ex)
            {

                throw ex;
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
                CustomSettings TempSettings=null;
                if (File.Exists(_ConfigFullFileName))
                {
                    
                    using (FileStream oFileStream = new FileStream(_ConfigFullFileName, FileMode.Open))
                    {
                        XmlSerializer oXmlSerializer = new XmlSerializer(typeof(CustomSettings));
                        TempSettings = (CustomSettings)oXmlSerializer.Deserialize(oFileStream);
                    }
                }

                if (TempSettings != null)
                    CoreManager.ConfigManager.Settings = TempSettings;
                else
                    throw new Exception();
            }
            catch
            {
                CoreManager.ConfigManager.Settings = new CustomSettings();
                SaveSettings();
            }
        }

    }
}

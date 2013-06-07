using System;
using System.IO;
using CSNovelCrawler.Core;

namespace CSNovelCrawler.Class
{
    public class FileWrite
    {
        public static void TxtWrire(string txt, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    throw new FileLoadException("目錄存取有誤");

                string directoryName = Path.GetDirectoryName(fileName);

                if (string.IsNullOrEmpty(directoryName))
                    throw new FileLoadException("目錄存取有誤");

                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.Write(txt);
                }

            }
            catch (Exception ex)
            {
                CoreManager.LogManager.Debug(ex.ToString());
            }
        }
    }
}

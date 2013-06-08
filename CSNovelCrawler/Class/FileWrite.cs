using System;
using System.IO;
using System.Text;
using CSNovelCrawler.Core;

namespace CSNovelCrawler.Class
{
    public class FileWrite
    {
        public static Encoding GetFileEncoding(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return Encoding.Unicode;
            }
            using (var br = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read)))
            {
                Byte[] buffer = br.ReadBytes(2);
                if (buffer[0] >= 0xEF)
                {
                    if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                    {
                        return Encoding.UTF8;
                    }
                    if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                    {
                        return Encoding.BigEndianUnicode;
                    }
                    if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                    {
                        return Encoding.Unicode;
                    }

                }
            }

            return Encoding.Default;
        }
        public static void TxtWrire(string txt, string fileName,Encoding textEncoding)
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


                if (textEncoding == null) textEncoding = GetFileEncoding(fileName);


                using (var sw = new StreamWriter(new FileStream(fileName, FileMode.Append), textEncoding))
                {
                    sw.Write(txt);
                    
                }

            }
            catch (Exception ex)
            {
                CoreManager.LogManager.Debug(ex.ToString());
                throw ex;
            }
        }
    }
}

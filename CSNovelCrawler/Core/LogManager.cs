using System;
using System.IO;

namespace CSNovelCrawler.Core
{
    public class LogManager
    {
        private string DirectoryName { get; set; }
        private string FullPath {
            get { return Path.Combine(DirectoryName, "log.txt"); }
        }

        public void Debug(string format, params object[] arg)
        {
            Write(string.Format(format, arg));
        }

        private void Write(string message)
        {
            if (string.IsNullOrEmpty(DirectoryName))
            {
                DirectoryName = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);

            var writeString = string.Format("{0:yyyy/MM/dd HH:mm:ss} {1}",
                DateTime.Now, message) + Environment.NewLine;

            using (StreamWriter sw = File.AppendText(FullPath))
            {
                sw.Write(writeString);
            }
          

        }
    }
}

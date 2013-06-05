using System;
using System.IO;

namespace CSNovelCrawler.Class
{
    public class FileWrite
    {
        public static void TxtWrire(string txt, string fileName)
        {
            try
            {
                // Create a new file 
                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.Write(txt);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

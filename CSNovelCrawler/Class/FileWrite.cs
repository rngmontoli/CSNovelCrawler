using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSNovelCrawler
{
    public class FileWrite
    {
        public static void TxtWrire(string Txt, string fileName)
        {
            try
            {
                // Create a new file 
                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.Write(Txt);
                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}

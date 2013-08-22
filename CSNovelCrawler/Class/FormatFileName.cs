using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CSNovelCrawler.Class
{
    public class FormatFileName
    {

        public string OutputFormat(TaskInfo taskInfo, string Format)
        {
            //Regex rx = new Regex(@"(?<Start>.*?)(%(?<Format>\w+)%)*(?<End>.*?)");
            Regex rx = new Regex(@"(?<Format>%\w+%)");
            string result = Format;
            try
            {
                //foreach (Match match in rx.Matches(Format))
                //{
                //    result += match.Groups["Start"].Value;
                //    string mFormat = match.Groups["Format"].Value;
                //    if (string.IsNullOrEmpty(mFormat))
                //    {
                //        mFormat = "?";
                //    }
                //    result += GetProperties<TaskInfo>(taskInfo, mFormat);
                //    result += match.Groups["End"].Value;
                //}
                foreach (Match match in rx.Matches(Format))
                {
                    //result += match.Groups["Start"].Value;
                    string mFormat = match.Groups["Format"].Value;
                    result = result.Replace(mFormat, GetProperties<TaskInfo>(taskInfo, mFormat.Trim('%')));
                    //result += match.Groups["End"].Value;
                }
            }
            catch(Exception ex)
            {
                result = "格式化檔名出現錯誤了";
            }
            
            return result;
        }

        private string GetProperties<T>(T t, string Propertyname)
        {
            if (t == null)
            {
                return "?";
            }

            PropertyInfo prop = t.GetType().GetProperty(Propertyname); // Simpler.

            if (prop == null)
            {
                return "?";
            }

            if (!prop.PropertyType.Name.StartsWith("String"))
            {
                return "?";
            }

            return prop.GetValue(t, null).ToString();

        }
    }
}

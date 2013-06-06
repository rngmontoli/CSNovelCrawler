using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace CSNovelCrawler.Class
{
    [SerializableAttribute]
    public class DictionaryExtension<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {


        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)                                               //由 XML 反列化為物件
        {
            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == "Item")
                    {
                        TKey key = (TKey)GetObjectFromXML(reader.GetAttribute("Key"), typeof(TKey));
                        TValue value = (TValue)GetObjectFromXML(reader.GetAttribute("Value"), typeof(TValue));
                        Add(key, value);
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer)                                              //將物件序列化為 XML
        {
            foreach (TKey key in Keys)
            {
                writer.WriteStartElement("Item");
                writer.WriteAttributeString("Key", GetXMLFromObject(key));
                writer.WriteAttributeString("Value", GetXMLFromObject(this[key]));
                writer.WriteEndElement();
            }
        }

        private string GetXMLFromObject(object source)                                      //將物件轉為 XML (僅供 DictionaryExtension 使用)
        {
            XmlSerializer XmlSerializer = new XmlSerializer(source.GetType());
            StringBuilder XmlWriterBuf = new StringBuilder();
            XmlSerializer.Serialize(XmlWriter.Create(XmlWriterBuf), source);

            return XmlWriterBuf.ToString();
        }
        private object GetObjectFromXML(string xmlData, Type objectType)                    //將 XML 轉為物件 (僅供 DictionaryExtension 使用)
        {
            XmlSerializer XmlSerializer = new XmlSerializer(objectType);
            StringReader XmlData = new StringReader(xmlData);

            return XmlSerializer.Deserialize(XmlData);
        }

        
    }
}
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace CSNovelCrawler
{
    public class DictionaryExtension<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader Reader)                                               //由 XML 反列化為物件
        {
            TKey Key;
            TValue Value;

            if (Reader.IsEmptyElement)
                return;

            while (Reader.Read())
            {
                if (Reader.NodeType == XmlNodeType.Element)
                {
                    if (Reader.LocalName == "Item")
                    {
                        Key = (TKey)GetObjectFromXML(Reader.GetAttribute("Key"), typeof(TKey));
                        Value = (TValue)GetObjectFromXML(Reader.GetAttribute("Value"), typeof(TValue));
                        base.Add(Key, Value);
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer)                                              //將物件序列化為 XML
        {
            foreach (TKey Key in this.Keys)
            {
                writer.WriteStartElement("Item");
                writer.WriteAttributeString("Key", GetXMLFromObject(Key));
                writer.WriteAttributeString("Value", GetXMLFromObject(this[Key]));
                writer.WriteEndElement();
            }
        }

        private string GetXMLFromObject(object Source)                                      //將物件轉為 XML (僅供 DictionaryExtension 使用)
        {
            XmlSerializer XmlSerializer = new XmlSerializer(Source.GetType());
            StringBuilder XmlWriterBuf = new StringBuilder();
            XmlSerializer.Serialize(XmlWriter.Create(XmlWriterBuf), Source);

            return XmlWriterBuf.ToString();
        }
        private object GetObjectFromXML(string XMLData, Type ObjectType)                    //將 XML 轉為物件 (僅供 DictionaryExtension 使用)
        {
            XmlSerializer XmlSerializer = new XmlSerializer(ObjectType);
            StringReader XmlData = new StringReader(XMLData);

            return XmlSerializer.Deserialize(XmlData);
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SavedContentsManager.utils
{
    /// <summary>
    /// http://stackoverflow.com/questions/12554186/how-to-serialize-deserialize-to-dictionaryint-string-from-custom-xml-not-us
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void Load(string path, string fileName)
        {
            if (File.Exists(path + Path.DirectorySeparatorChar + fileName))
            {
                using (XmlReader reader = XmlReader.Create(path + Path.DirectorySeparatorChar + fileName))
                {
                    ReadXml(reader);
                }
            }
        }

        public void Save(string path, string fileName)
        {
            using (XmlWriter writer = XmlWriter.Create(path + Path.DirectorySeparatorChar + fileName))
            {
                WriteXml(writer);
            }
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            reader.ReadStartElement("item");

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            writer.WriteStartElement("item");

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
        #endregion
    }
}

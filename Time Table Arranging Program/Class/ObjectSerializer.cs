using ExtraTools;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Class {
    internal class ObjectSerializer {
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public bool SerializeObject<T>(T serializableObject, string fileName) {
            if (serializableObject == null) {
                return false;
            }

            try {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream()) {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
                return true;
            }
            catch (Exception ex) {
                DialogBox.Show("Oops.. Couldn't save the file", ex.Message, "OK");
                // Exception inner = ex.InnerException;
                //do {
                //    MessageBox.Show(inner.Message);
                //} while (((inner = inner.InnerException) != null)); 
                return false;
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeSerializeObject<T>(string fileName) {
            if (string.IsNullOrEmpty(fileName)) {
                return default(T);
            }

            T objectOut = default(T);

            try {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString)) {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read)) {
                        objectOut = (T) serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex) {
                //Log exception here
            }

            return objectOut;
        }
    }
}
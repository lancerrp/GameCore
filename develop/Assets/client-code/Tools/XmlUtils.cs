using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class XmlUtils
{
    public static T GetXMLData<T>(string filePath) where T : class
    {
        T result = null;
        FileStream fileStream = null;
        System.Xml.XmlReader reader = null;
        Type type = typeof(T);
        try
        {
            fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            reader = System.Xml.XmlReader.Create(fileStream);
            if (type == null)
            {
                return null;
            }
            if (type == typeof(T) || type.IsSubclassOf(typeof(T)))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);
                result = (T)serializer.Deserialize(reader);
            }
        }
        catch (Exception ex)
        {
            System.Xml.XmlTextReader textReader = reader as System.Xml.XmlTextReader;
            if (textReader != null)
            {
                Debug.LogError(string.Format("GetXMLData Failed At {0}:{1},{2}", filePath, textReader.LineNumber, textReader.LinePosition));
            }
            Debug.LogException(ex);
        }
        if (reader != null)
        {
            reader.Close();
        }
        if (fileStream != null)
        {
            fileStream.Close();
        }
        return result;
    }

    public static object GetXMLData(Type type, string filePath) 
    {
        object result = null;
        FileStream fileStream = null;
        System.Xml.XmlReader reader = null;
        try
        {
            fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            reader = System.Xml.XmlReader.Create(fileStream);
            if (type == null)
            {
                return null;
            }
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);
            result = serializer.Deserialize(reader);
        }
        catch (Exception ex)
        {
            System.Xml.XmlTextReader textReader = reader as System.Xml.XmlTextReader;
            if (textReader != null)
            {
                Debug.LogError(string.Format("GetXMLData Failed At {0}:{1},{2}", filePath, textReader.LineNumber, textReader.LinePosition));
            }
            Debug.LogException(ex);
        }
        if (reader != null)
        {
            reader.Close();
        }
        if (fileStream != null)
        {
            fileStream.Close();
        }
        return result;
    }
}

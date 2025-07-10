//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	Utilities.cs
// Author Name		:	Pramod Prakash Pokale
// Employee ID		:	
// Email			:	
// Contact No		:	
// Creation Time	:	10/29/2015
// Description  	:	Provides functions which can be used across YRS objects.
//*******************************************************************************
//Modified By           Date            Description
//*******************************************************************************
//
//*******************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace YMCARET.CommonUtilities
{
    public class Utilities
    {
        /// <summary>
        /// Converts <paramref name="T"/>list of objects into xml format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Converted XML in string format</returns>
        public static string SerializeToXML<T>(List<T> source)
        {
            if (source == null)
                return null;

            XmlSerializer serializer = new XmlSerializer(source.GetType());

            StringWriter _StringWriter = new StringWriter();
            //XmlTextWriter _XmlTextWriter = new XmlTextWriter(_StringWriter);

            //Setting for xml
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlWriter _XmlWriter = XmlWriter.Create(_StringWriter, settings);

            //Remove Qualifier fields from nodes
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(_XmlWriter, source, emptyNs);

            return _StringWriter.ToString();
        }

        /// <summary>
        /// Converts <paramref name="T"/> object into xml format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Converted XML in string format</returns>
        public static string SerializeToXML<T>(T source)
        {
            if (source == null)
                return null;

            XmlSerializer serializer = new XmlSerializer(source.GetType());

            StringWriter _StringWriter = new StringWriter();
            //XmlTextWriter _XmlTextWriter = new XmlTextWriter(_StringWriter);

            //Setting for xml
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlWriter _XmlWriter = XmlWriter.Create(_StringWriter, settings);

            //Remove Qualifier fields from nodes
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(_XmlWriter, source, emptyNs);

            return _StringWriter.ToString();
        }

    }
}

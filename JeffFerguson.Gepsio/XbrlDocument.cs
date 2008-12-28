using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio
{
    public class XbrlDocument
    {
        public static string XbrlNamespaceUri = "http://www.xbrl.org/2003/instance";

        private List<XbrlFragment> thisXbrlFragments;
        private string thisFilename;
        private string thisPath;

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string Filename
        {
            get
            {
                return thisFilename;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string Path
        {
            get
            {
                return thisPath;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public XbrlDocument()
        {
            thisXbrlFragments = new List<XbrlFragment>();
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public void Load(string Filename)
        {
            XmlDocument SchemaValidXbrl = new XmlDocument();
            SchemaValidXbrl.Load(Filename);
            thisFilename = Filename;
            thisPath = System.IO.Path.GetDirectoryName(thisFilename);
            XmlNamespaceManager NewNamespaceManager = new XmlNamespaceManager(SchemaValidXbrl.NameTable);
            NewNamespaceManager.AddNamespace("instance", XbrlNamespaceUri);
            XmlNodeList XbrlNodes = SchemaValidXbrl.SelectNodes("//instance:xbrl", NewNamespaceManager);
            foreach (XmlNode XbrlNode in XbrlNodes)
                thisXbrlFragments.Add(new XbrlFragment(this, XbrlNode));
        }
    }
}

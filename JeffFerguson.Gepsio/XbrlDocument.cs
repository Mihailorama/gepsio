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

        //===============================================================================
        #region XML Schema Validation
        //===============================================================================
        [Obsolete()]
        private void ValidateDocument(string Filename)
        {
            Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();
            Stream InstanceSchemaResourceStream = ExecutingAssembly.GetManifestResourceStream("JeffFerguson.Xbrl.xbrl-instance-2003-12-31.xsd");
            XmlTextReader XbrlInstanceSchema = new XmlTextReader(InstanceSchemaResourceStream);
            Stream LinkbaseSchemaResourceStream = ExecutingAssembly.GetManifestResourceStream("JeffFerguson.Xbrl.xbrl-linkbase-2003-12-31.xsd");
            XmlTextReader XbrlLinkbaseSchema = new XmlTextReader(LinkbaseSchemaResourceStream);
            Stream XlSchemaResourceStream = ExecutingAssembly.GetManifestResourceStream("JeffFerguson.Xbrl.xl-2003-12-31.xsd");
            XmlTextReader XbrlXlSchema = new XmlTextReader(XlSchemaResourceStream);
            Stream XlinkSchemaResourceStream = ExecutingAssembly.GetManifestResourceStream("JeffFerguson.Xbrl.xlink-2003-12-31.xsd");
            XmlTextReader XbrlXlinkSchema = new XmlTextReader(XlinkSchemaResourceStream);

            XmlReaderSettings Settings = new XmlReaderSettings();
            Settings.Schemas.Add(null, XbrlInstanceSchema);
            Settings.Schemas.Add(null, XbrlLinkbaseSchema);
            Settings.Schemas.Add(null, XbrlXlSchema);
            Settings.Schemas.Add(null, XbrlXlinkSchema);
            Settings.ValidationType = ValidationType.Schema;
            Settings.ValidationEventHandler += new ValidationEventHandler(Settings_ValidationEventHandler);

            StreamReader XbrlStream = new StreamReader(Filename);
            XmlReader XbrlTextReader;

            try
            {
                XbrlTextReader = XmlReader.Create(XbrlStream, Settings);
            }
            catch (Exception e)
            {
                string Message = AssemblyResources.GetName("ErrorLoadingXbrlDoc");
                throw new XbrlException(Message, e);
            }
        }

        [Obsolete()]
        private void Settings_ValidationEventHandler(object sender, EventArgs e)
        {
            string Message = AssemblyResources.GetName("ErrorLoadingXbrlDoc");
            throw new XbrlException(Message);
        }

        //===============================================================================
        #endregion
        //===============================================================================
    }
}

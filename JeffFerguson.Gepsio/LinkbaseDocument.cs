using System.Collections.Generic;
using System.Xml;
using System;

namespace JeffFerguson.Gepsio
{
    //========================================================================================
    // Represents an XML document with a root element called <linkbase>. Linkbase documents
    // are referenced in <linkbaseRef> elements in XBRL schemas.
    //========================================================================================
    public class LinkbaseDocument
    {
        private XbrlSchema thisContainingXbrlSchema;
        private XmlDocument thisXmlDocument;
        private string thisLinkbasePath;
        private XmlNamespaceManager thisNamespaceManager;
        private XmlNode thisLinkbaseNode;
        private List<DefinitionLink> thisDefinitionLinks;
        private List<CalculationLink> thisCalculationLinks;
        private List<LabelLink> thisLabelLinks;

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public List<DefinitionLink> DefinitionLinks
        {
            get
            {
                return thisDefinitionLinks;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public List<CalculationLink> CalculationLinks
        {
            get
            {
                return thisCalculationLinks;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public List<LabelLink> LabelLinks
        {
            get
            {
                return thisLabelLinks;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal LinkbaseDocument(XbrlSchema ContainingXbrlSchema, string DocumentPath)
        {
            thisDefinitionLinks = new List<DefinitionLink>();
            thisCalculationLinks = new List<CalculationLink>();
            thisLabelLinks = new List<LabelLink>();
            thisContainingXbrlSchema = ContainingXbrlSchema;
            thisLinkbasePath = GetFullLinkbasePath(DocumentPath);
            thisXmlDocument = new XmlDocument();
            thisXmlDocument.Load(thisLinkbasePath);
            thisNamespaceManager = new XmlNamespaceManager(thisXmlDocument.NameTable);
            thisNamespaceManager.AddNamespace("default", "http://www.xbrl.org/2003/linkbase");
            ReadLinkbaseNode();
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadLinkbaseNode()
        {
            thisLinkbaseNode = thisXmlDocument.SelectSingleNode("//default:linkbase", thisNamespaceManager);
            foreach (XmlNode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("definitionLink") == true)
                    thisDefinitionLinks.Add(new DefinitionLink(CurrentChild));
                else if (CurrentChild.LocalName.Equals("calculationLink") == true)
                    thisCalculationLinks.Add(new CalculationLink(CurrentChild));
                else if (CurrentChild.LocalName.Equals("labelLink") == true)
                    thisLabelLinks.Add(new LabelLink(CurrentChild));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private string GetFullLinkbasePath(string LinkbaseDocFilename)
        {
            string FullPath;
            int FirstPathSeparator = LinkbaseDocFilename.IndexOf(System.IO.Path.DirectorySeparatorChar);
            if (FirstPathSeparator == -1)
            {
                string DocumentUri = thisContainingXbrlSchema.SchemaRootNode.BaseURI;
                int LastPathSeparator = DocumentUri.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                if (LastPathSeparator == -1)
                    LastPathSeparator = DocumentUri.LastIndexOf('/');
                string DocumentPath = DocumentUri.Substring(0, LastPathSeparator + 1);
                FullPath = DocumentPath + LinkbaseDocFilename;
            }
            else
            {
                throw new NotImplementedException("XbrlSchema.GetFullSchemaPath() code path not implemented.");
            }
            return FullPath;
        }
    }
}

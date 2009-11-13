using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class XbrlSchema
    {
        private XmlDocument thisSchemaDocument;
        private XbrlFragment thisContainingXbrlFragment;
        private XmlNode thisSchemaNode;
        private List<Element> thisElements;
        private UriPrefixDictionary thisUriPrefixDictionary;
        private string thisSchemaPath;
        private string thisTargetNamespace;
        private List<SimpleType> thisSimpleTypes;
        private List<ComplexType> thisComplexTypes;
        private XmlNamespaceManager thisNamespaceManager;
        private List<LinkbaseDocument> thisLinkbaseDocuments;

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string Path
        {
            get
            {
                return thisSchemaPath;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal UriPrefixDictionary UrisAndPrefixes
        {
            get
            {
                return thisUriPrefixDictionary;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal XmlNode SchemaRootNode
        {
            get
            {
                return thisSchemaNode;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string TargetNamespace
        {
            get
            {
                return thisTargetNamespace;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<Element> Elements
        {
            get
            {
                return thisElements;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<SimpleType> SimpleTypes
        {
            get
            {
                return thisSimpleTypes;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<ComplexType> ComplexTypes
        {
            get
            {
                return thisComplexTypes;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<LinkbaseDocument> LinkbaseDocuments
        {
            get
            {
                return thisLinkbaseDocuments;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public XmlNamespaceManager NamespaceManager
        {
            get
            {
                return thisNamespaceManager;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal XbrlSchema(XbrlFragment ContainingXbrlFragment, string SchemaFilename, string BaseDirectory)
        {
            thisContainingXbrlFragment = ContainingXbrlFragment;
            thisSchemaPath = GetFullSchemaPath(SchemaFilename, BaseDirectory);
            thisSchemaDocument = new XmlDocument();
            thisLinkbaseDocuments = new List<LinkbaseDocument>();
            thisSchemaDocument.Load(thisSchemaPath);
            thisNamespaceManager = new XmlNamespaceManager(thisSchemaDocument.NameTable);
            thisNamespaceManager.AddNamespace("schema", "http://www.w3.org/2001/XMLSchema");
            ReadSchemaNode();
            ReadSimpleTypes();
            ReadComplexTypes();
            ReadElements();
            LookForAnnotations();
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public Element GetElement(string ElementName)
        {
            foreach (Element CurrentElement in thisElements)
            {
                if (CurrentElement.Name.Equals(ElementName) == true)
                    return CurrentElement;
            }
            return null;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private string GetFullSchemaPath(string SchemaFilename, string BaseDirectory)
        {
            string FullPath;
            int FirstPathSeparator = SchemaFilename.IndexOf(System.IO.Path.DirectorySeparatorChar);
            if (FirstPathSeparator == -1)
            {
                string DocumentUri = thisContainingXbrlFragment.XbrlRootNode.BaseURI;
                int LastPathSeparator = DocumentUri.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                if (LastPathSeparator == -1)
                    LastPathSeparator = DocumentUri.LastIndexOf('/');
                string DocumentPath = DocumentUri.Substring(0, LastPathSeparator + 1);
                if (BaseDirectory.Length > 0)
                    DocumentPath = DocumentPath + BaseDirectory;
                FullPath = DocumentPath + SchemaFilename;
            }
            else
            {
                throw new NotImplementedException("XbrlSchema.GetFullSchemaPath() code path not implemented.");
            }
            return FullPath;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadSchemaNode()
        {
            thisElements = new List<Element>();
            thisSchemaNode = thisSchemaDocument.SelectSingleNode("//schema:schema", thisNamespaceManager);
            if (thisSchemaNode == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("SchemaFileCandidateDoesNotContainSchemaRootNode");
                MessageBuilder.AppendFormat(StringFormat, thisSchemaPath);
                throw new XbrlException(MessageBuilder.ToString());
            }
            thisTargetNamespace = thisSchemaNode.Attributes["targetNamespace"].Value;
            foreach (XmlAttribute CurrentAttribute in thisSchemaNode.Attributes)
                if (CurrentAttribute.Prefix == "xmlns")
                    thisNamespaceManager.AddNamespace(CurrentAttribute.LocalName, CurrentAttribute.Value);
            thisUriPrefixDictionary = new UriPrefixDictionary();
            thisUriPrefixDictionary.Load(thisSchemaNode);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadSimpleTypes()
        {
            thisSimpleTypes = new List<SimpleType>();
            XmlNodeList SimpleTypeNodes = thisSchemaDocument.SelectNodes("//schema:simpleType", thisNamespaceManager);
            foreach (XmlNode SimpleTypeNode in SimpleTypeNodes)
                thisSimpleTypes.Add(new SimpleType(SimpleTypeNode));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadComplexTypes()
        {
            thisComplexTypes = new List<ComplexType>();
            XmlNodeList ComplexTypeNodes = thisSchemaDocument.SelectNodes("//schema:complexType", thisNamespaceManager);
            foreach (XmlNode ComplexTypeNode in ComplexTypeNodes)
                thisComplexTypes.Add(new ComplexType(ComplexTypeNode));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadElements()
        {
            foreach (XmlNode CurrentChild in thisSchemaNode.ChildNodes)
            {
                if(CurrentChild.LocalName.Equals("element") == true)
                    thisElements.Add(new Element(this, CurrentChild));
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal SimpleType GetSimpleType(string ItemTypeValue)
        {
            foreach (SimpleType CurrentSimpleType in this.SimpleTypes)
            {
                if (CurrentSimpleType.Name.Equals(ItemTypeValue) == true)
                    return CurrentSimpleType;
            }
            return null;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal ComplexType GetComplexType(string ItemTypeValue)
        {
            foreach (ComplexType CurrentComplexType in this.ComplexTypes)
            {
                if (CurrentComplexType.Name.Equals(ItemTypeValue) == true)
                    return CurrentComplexType;
            }
            return null;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void LookForAnnotations()
        {
            foreach (XmlNode CurrentChild in thisSchemaNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("annotation") == true)
                    ReadAnnotations(CurrentChild);
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadAnnotations(XmlNode AnnotationNode)
        {
            foreach (XmlNode CurrentChild in AnnotationNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("appinfo") == true)
                    ReadAppInfo(CurrentChild);
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadAppInfo(XmlNode AppInfoNode)
        {
            foreach (XmlNode CurrentChild in AppInfoNode.ChildNodes)
            {
                if ((CurrentChild.NamespaceURI.Equals("http://www.xbrl.org/2003/linkbase") == true) && (CurrentChild.LocalName.Equals("linkbaseRef") == true))
                {
                    ReadLinkbaseReference(CurrentChild);
                }
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadLinkbaseReference(XmlNode LinkbaseReferenceNode)
        {
            foreach (XmlAttribute CurrentAttribute in LinkbaseReferenceNode.Attributes)
            {
                if ((CurrentAttribute.NamespaceURI.Equals("http://www.w3.org/1999/xlink") == true) && (CurrentAttribute.LocalName.Equals("href") == true))
                    thisLinkbaseDocuments.Add(new LinkbaseDocument(this, CurrentAttribute.Value));
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal Element LocateElement(Locator ElementLocator)
        {
            foreach (Element CurrentElement in thisElements)
            {
                if (CurrentElement.Id.Equals(ElementLocator.HrefResourceId) == true)
                    return CurrentElement;
            }
            return null;
        }
    }
}

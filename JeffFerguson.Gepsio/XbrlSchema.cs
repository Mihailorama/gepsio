using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Collections;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A representation of all of the information in an XBRL schema file.
    /// </summary>
    public class XbrlSchema
    {
        private XmlDocument thisSchemaDocument;
        private XbrlFragment thisContainingXbrlFragment;
        private XmlNode thisSchemaNode;
        private List<Element> thisElements;
        private string thisSchemaPath;
        private string thisTargetNamespace;
        private List<SimpleType> thisSimpleTypes;
        private List<ComplexType> thisComplexTypes;
        private XmlNamespaceManager thisNamespaceManager;
        private List<LinkbaseDocument> thisLinkbaseDocuments;

        private XmlSchema thisXmlSchema;
        private XmlSchemaSet thisXmlSchemaSet;

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

            try
            {
                thisXmlSchema = XmlSchema.Read(XmlTextReader.Create(thisSchemaPath), null);
                thisXmlSchemaSet = new XmlSchemaSet();
                thisXmlSchemaSet.Add(thisXmlSchema);
                thisXmlSchemaSet.Compile();
            }
            catch (XmlSchemaException)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("SchemaFileCandidateDoesNotContainSchemaRootNode");
                MessageBuilder.AppendFormat(StringFormat, thisSchemaPath);
                throw new XbrlException(MessageBuilder.ToString());
            }

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

        /// <summary>
        /// Reads the schema's root node and collects namespace data from the namespace attributes.
        /// </summary>
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
            //foreach (XmlNode CurrentChild in thisSchemaNode.ChildNodes)
            //{
            //    if (CurrentChild.LocalName.Equals("element") == true)
            //        thisElements.Add(new Element(this, CurrentChild));
            //}
            foreach (DictionaryEntry CurrentEntry in thisXmlSchemaSet.GlobalElements)
            {
                XmlSchemaElement CurrentElement = CurrentEntry.Value as XmlSchemaElement;
                thisElements.Add(new Element(this, CurrentElement));
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal XmlSchemaType GetXmlSchemaType(XmlQualifiedName ItemTypeValue)
        {
            foreach (DictionaryEntry CurrentEntry in thisXmlSchemaSet.GlobalTypes)
            {
                XmlSchemaType CurrentType = CurrentEntry.Value as XmlSchemaType;
                if (CurrentType.QualifiedName.Equals(ItemTypeValue) == true)
                {
                    return CurrentType;
                }
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
                if (string.IsNullOrEmpty(CurrentElement.Id) == false)
                {
                    if (CurrentElement.Id.Equals(ElementLocator.HrefResourceId) == true)
                        return CurrentElement;
                }
            }
            return null;
        }

        /// <summary>
        /// Given a URI, return the namespace prefix associated with the URI.
        /// </summary>
        /// <param name="uri">A namespace URI.</param>
        /// <returns>A string representing the namespace prefix. An empty string is returned if the URI is not defined in the schema.</returns>
        internal string GetPrefixForUri(string uri)
        {
            XmlQualifiedName[] NamespacesArray = thisXmlSchema.Namespaces.ToArray();
            foreach (XmlQualifiedName CurrentName in NamespacesArray)
            {
                if (CurrentName.Namespace.Equals(uri) == true)
                    return CurrentName.Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// Given a namespace prefix, return the URI associated with the namespace.
        /// </summary>
        /// <param name="prefix">A namespace prefix.</param>
        /// <returns>The URI associated with the namespace.</returns>
        internal string GetUriForPrefix(string prefix)
        {
            XmlQualifiedName[] NamespacesArray = thisXmlSchema.Namespaces.ToArray();
            foreach (XmlQualifiedName CurrentName in NamespacesArray)
            {
                if (CurrentName.Name.Equals(prefix) == true)
                    return CurrentName.Namespace;
            }
            return string.Empty;
        }
    }
}

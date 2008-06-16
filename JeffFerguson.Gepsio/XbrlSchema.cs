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

        public string Path
        {
            get
            {
                return thisSchemaPath;
            }
        }

        internal UriPrefixDictionary UrisAndPrefixes
        {
            get
            {
                return thisUriPrefixDictionary;
            }
        }

        public string TargetNamespace
        {
            get
            {
                return thisTargetNamespace;
            }
        }

        public List<Element> Elements
        {
            get
            {
                return thisElements;
            }
        }

        public List<SimpleType> SimpleTypes
        {
            get
            {
                return thisSimpleTypes;
            }
        }

        public List<ComplexType> ComplexTypes
        {
            get
            {
                return thisComplexTypes;
            }
        }

        internal XbrlSchema(XbrlFragment ContainingXbrlFragment, string SchemaFilename)
        {
            thisContainingXbrlFragment = ContainingXbrlFragment;
            thisSchemaPath = GetFullSchemaPath(SchemaFilename);
            thisSchemaDocument = new XmlDocument();
            thisSchemaDocument.Load(thisSchemaPath);
            thisNamespaceManager = new XmlNamespaceManager(thisSchemaDocument.NameTable);
            thisNamespaceManager.AddNamespace("schema", "http://www.w3.org/2001/XMLSchema");
            ReadSchemaNode();
            ReadSimpleTypes();
            ReadComplexTypes();
            ReadElements();
        }

        public Element GetElement(string ElementName)
        {
            foreach (Element CurrentElement in thisElements)
            {
                if (CurrentElement.Name.Equals(ElementName) == true)
                    return CurrentElement;
            }
            return null;
        }

        private string GetFullSchemaPath(string SchemaFilename)
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
                FullPath = DocumentPath + SchemaFilename;
            }
            else
            {
                throw new NotImplementedException("XbrlSchema.GetFullSchemaPath() code path not implemented.");
            }
            return FullPath;
        }

        private void ReadSchemaNode()
        {
            thisElements = new List<Element>();
            thisSchemaNode = thisSchemaDocument.SelectSingleNode("//schema:schema", thisNamespaceManager);
            thisTargetNamespace = thisSchemaNode.Attributes["targetNamespace"].Value;
            thisUriPrefixDictionary = new UriPrefixDictionary();
            thisUriPrefixDictionary.Load(thisSchemaNode);
        }

        private void ReadSimpleTypes()
        {
            thisSimpleTypes = new List<SimpleType>();
            XmlNodeList SimpleTypeNodes = thisSchemaDocument.SelectNodes("//schema:simpleType", thisNamespaceManager);
            foreach (XmlNode SimpleTypeNode in SimpleTypeNodes)
                thisSimpleTypes.Add(SimpleType.CreateSimpleType(SimpleTypeNode));
        }

        private void ReadComplexTypes()
        {
            thisComplexTypes = new List<ComplexType>();
            XmlNodeList ComplexTypeNodes = thisSchemaDocument.SelectNodes("//schema:complexType", thisNamespaceManager);
            foreach (XmlNode ComplexTypeNode in ComplexTypeNodes)
                thisComplexTypes.Add(ComplexType.CreateComplexType(ComplexTypeNode));
        }

        private void ReadElements()
        {
            foreach (XmlNode CurrentChild in thisSchemaNode.ChildNodes)
            {
                if(CurrentChild.LocalName.Equals("element") == true)
                    thisElements.Add(new Element(this, CurrentChild));
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A representation of all of the information in an XBRL schema file.
    /// </summary>
    public class XbrlSchema
    {
        private XmlDocument thisSchemaDocument;
        private XbrlFragment thisContainingXbrlFragment;
        private XmlSchema thisXmlSchema;
        private XmlSchemaSet thisXmlSchemaSet;

		/// <summary>
		/// The full path to the XBRL schema file.
		/// </summary>
		public string Path
		{
			get;
			private set;
		}

		/// <summary>
		/// The root node of the parsed schema document.
		/// </summary>
		internal XmlNode SchemaRootNode
		{
			get;
			private set;
		}

		/// <summary>
		/// The target namespace of the schema.
		/// </summary>
		public string TargetNamespace
		{
			get;
			private set;
		}

		/// <summary>
		/// A collection of <see cref="Element"/> objects representing all elements defined in the schema.
		/// </summary>
		public List<Element> Elements
		{
			get;
			private set;
		}

		/// <summary>
		/// A collection of <see cref="SimpleType"/> objects representing all simple types defined in the schema.
		/// </summary>
		public List<SimpleType> SimpleTypes
		{
			get;
			private set;
		}

		/// <summary>
		/// A collection of <see cref="ComplexType"/> objects representing all complex types defined in the schema.
		/// </summary>
		public List<ComplexType> ComplexTypes
		{
			get;
			private set;
		}

		/// <summary>
		/// A collection of <see cref="LinkbaseDocument"/> objects representing all linkbase documents defined in the schema.
		/// </summary>
		public List<LinkbaseDocument> LinkbaseDocuments
		{
			get;
			private set;
		}

		/// <summary>
		/// The namespace manager associated with the parsed schema document.
		/// </summary>
		public XmlNamespaceManager NamespaceManager
		{
			get;
			private set;
		}

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal XbrlSchema(XbrlFragment ContainingXbrlFragment, string SchemaFilename, string BaseDirectory)
        {
            thisContainingXbrlFragment = ContainingXbrlFragment;
            this.Path = GetFullSchemaPath(SchemaFilename, BaseDirectory);

            try
            {
				thisXmlSchema = XmlSchema.Read(XmlTextReader.Create(this.Path), null);
                thisXmlSchemaSet = new XmlSchemaSet();
                thisXmlSchemaSet.Add(thisXmlSchema);
                thisXmlSchemaSet.Compile();
            }
            catch (XmlSchemaException)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("SchemaFileCandidateDoesNotContainSchemaRootNode");
                MessageBuilder.AppendFormat(StringFormat, this.Path);
                throw new XbrlException(MessageBuilder.ToString());
            }

            thisSchemaDocument = new XmlDocument();
            this.LinkbaseDocuments = new List<LinkbaseDocument>();
            thisSchemaDocument.Load(this.Path);
            this.NamespaceManager = new XmlNamespaceManager(thisSchemaDocument.NameTable);
            this.NamespaceManager.AddNamespace("schema", "http://www.w3.org/2001/XMLSchema");
            ReadSchemaNode();
            ReadSimpleTypes();
            ReadComplexTypes();
            ReadElements();
            LookForAnnotations();
        }

		/// <summary>
		/// Gets the named element defined by the schema.
		/// </summary>
		/// <param name="ElementName">
		/// The name of the element to be returned.
		/// </param>
		/// <returns>
		/// A reference to the <see cref="Element"/> object representing the element with the given name.
		/// A null is returned if no <see cref="Element"/> object is available with the given name.
		/// </returns>
        public Element GetElement(string ElementName)
        {
            foreach (Element CurrentElement in this.Elements)
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
            this.Elements = new List<Element>();
            this.SchemaRootNode = thisSchemaDocument.SelectSingleNode("//schema:schema", this.NamespaceManager);
			if (this.SchemaRootNode == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("SchemaFileCandidateDoesNotContainSchemaRootNode");
                MessageBuilder.AppendFormat(StringFormat, this.Path);
                throw new XbrlException(MessageBuilder.ToString());
            }
			this.TargetNamespace = this.SchemaRootNode.Attributes["targetNamespace"].Value;
			foreach (XmlAttribute CurrentAttribute in this.SchemaRootNode.Attributes)
                if (CurrentAttribute.Prefix == "xmlns")
                    this.NamespaceManager.AddNamespace(CurrentAttribute.LocalName, CurrentAttribute.Value);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadSimpleTypes()
        {
            this.SimpleTypes = new List<SimpleType>();
            XmlNodeList SimpleTypeNodes = thisSchemaDocument.SelectNodes("//schema:simpleType", this.NamespaceManager);
            foreach (XmlNode SimpleTypeNode in SimpleTypeNodes)
                this.SimpleTypes.Add(new SimpleType(SimpleTypeNode));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadComplexTypes()
        {
            this.ComplexTypes = new List<ComplexType>();
            XmlNodeList ComplexTypeNodes = thisSchemaDocument.SelectNodes("//schema:complexType", this.NamespaceManager);
            foreach (XmlNode ComplexTypeNode in ComplexTypeNodes)
                this.ComplexTypes.Add(new ComplexType(ComplexTypeNode));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadElements()
        {
            foreach (DictionaryEntry CurrentEntry in thisXmlSchemaSet.GlobalElements)
            {
                XmlSchemaElement CurrentElement = CurrentEntry.Value as XmlSchemaElement;
                this.Elements.Add(new Element(this, CurrentElement));
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
			foreach (XmlNode CurrentChild in this.SchemaRootNode.ChildNodes)
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
                    this.LinkbaseDocuments.Add(new LinkbaseDocument(this, CurrentAttribute.Value));
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal Element LocateElement(Locator ElementLocator)
        {
            foreach (Element CurrentElement in this.Elements)
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

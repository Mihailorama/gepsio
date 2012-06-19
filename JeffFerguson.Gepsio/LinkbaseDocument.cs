using System;
using System.Collections.Generic;
using System.Xml;

namespace JeffFerguson.Gepsio
{
	/// <summary>
	/// Represents a linkbase document. A linkbase document is an XML document with a root
	/// element called linkbase. Linkbase documents are referenced in linkbaseRef elements in
	/// XBRL schemas.
	/// </summary>
    public class LinkbaseDocument
    {
        private XbrlSchema thisContainingXbrlSchema;
        private XmlDocument thisXmlDocument;
        private string thisLinkbasePath;
        private XmlNamespaceManager thisNamespaceManager;
        private XmlNode thisLinkbaseNode;

		/// <summary>
		/// A collection of <see cref="DefinitionLink"/> objects defined by the linkbase document.
		/// </summary>
		public List<DefinitionLink> DefinitionLinks
		{
			get;
			private set;
		}

		/// <summary>
		/// A collection of <see cref="CalculationLink"/> objects defined by the linkbase document.
		/// </summary>
		public List<CalculationLink> CalculationLinks
		{
			get;
			private set;
		}

		/// <summary>
		/// A collection of <see cref="LabelLink"/> objects defined by the linkbase document.
		/// </summary>
		public List<LabelLink> LabelLinks
		{
			get;
			private set;
		}

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal LinkbaseDocument(XbrlSchema ContainingXbrlSchema, string DocumentPath)
        {
            this.DefinitionLinks = new List<DefinitionLink>();
            this.CalculationLinks = new List<CalculationLink>();
            this.LabelLinks = new List<LabelLink>();
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
                    this.DefinitionLinks.Add(new DefinitionLink(CurrentChild));
                else if (CurrentChild.LocalName.Equals("calculationLink") == true)
                    this.CalculationLinks.Add(new CalculationLink(CurrentChild));
                else if (CurrentChild.LocalName.Equals("labelLink") == true)
                    this.LabelLinks.Add(new LabelLink(CurrentChild));
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

				// Check for remote linkbases when using local files

				if ((DocumentPath.StartsWith("file:///") == true) && (LinkbaseDocFilename.StartsWith("http://") == true))
					return LinkbaseDocFilename;

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

using JeffFerguson.Gepsio.IoC;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a linkbase document. A linkbase document is an XML document with a root
    /// element called linkbase. Linkbase documents are referenced in linkbaseRef elements in
    /// XBRL schemas.
    /// </summary>
    public class LinkbaseDocument
    {
        private IDocument thisXmlDocument;
        private string thisLinkbasePath;
        private INamespaceManager thisNamespaceManager;
        private INode thisLinkbaseNode;

        /// <summary>
        /// The schema that references this linkbase document.
        /// </summary>
        public XbrlSchema Schema
        {
            get;
            private set;
        }

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
            this.Schema = ContainingXbrlSchema;
            thisLinkbasePath = GetFullLinkbasePath(DocumentPath);
            thisXmlDocument = Container.Resolve<IDocument>();
            thisXmlDocument.Load(thisLinkbasePath);
            thisNamespaceManager = Container.Resolve<INamespaceManager>();
            thisNamespaceManager.Document = thisXmlDocument;
            thisNamespaceManager.AddNamespace("default", XbrlDocument.XbrlLinkbaseNamespaceUri);
            ReadLinkbaseNode();
        }

        /// <summary>
        /// Finds the <see cref="CalculationLink"/> object having the given role.
        /// </summary>
        /// <param name="CalculationLinkRole">
        /// The role type to find.
        /// </param>
        /// <returns>
        /// The <see cref="CalculationLink"/> object having the given role, or
        /// null if no object can be found.
        /// </returns>
        public CalculationLink GetCalculationLink(RoleType CalculationLinkRole)
        {
            foreach (var currentCalculationLink in CalculationLinks)
            {
                if (currentCalculationLink.RoleUri.Equals(CalculationLinkRole.RoleUri) == true)
                    return currentCalculationLink;
            }
            return null;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadLinkbaseNode()
        {
            thisLinkbaseNode = thisXmlDocument.SelectSingleNode("//default:linkbase", thisNamespaceManager);
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("definitionLink") == true)
                    this.DefinitionLinks.Add(new DefinitionLink(CurrentChild));
                else if (CurrentChild.LocalName.Equals("calculationLink") == true)
                    this.CalculationLinks.Add(new CalculationLink(this, CurrentChild));
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
                string DocumentUri = this.Schema.SchemaRootNode.BaseURI;
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

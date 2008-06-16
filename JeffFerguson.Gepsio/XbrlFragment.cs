using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace JeffFerguson.Gepsio
{
    public class XbrlFragment
    {
        //===============================================================================
        #region Delegates
        //===============================================================================
        public delegate void XbrlEventHandler(object sender, EventArgs e);
        //===============================================================================
        #endregion
        //===============================================================================

        //===============================================================================
        #region Events
        //===============================================================================
        public event XbrlEventHandler Loaded;
        public event XbrlEventHandler Validated;
        //===============================================================================
        #endregion
        //===============================================================================

        //===============================================================================
        #region Fields
        //===============================================================================
        private XbrlDocument thisDocument;
        private XmlNode thisXbrlRootNode;
        private List<Context> thisContexts;
        private XmlNamespaceManager thisNamespaceManager;
        private UriPrefixDictionary thisUriPrefixDictionary;
        private List<XbrlSchema> thisSchemas;
        private List<Fact> thisFacts;
        private List<Unit> thisUnits;
        private List<FootnoteLink> thisFootnoteLinks;
        //===============================================================================
        #endregion
        //===============================================================================

        //===============================================================================
        #region Properties
        //===============================================================================

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public XbrlDocument Document
        {
            get
            {
                return thisDocument;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public XmlNode XbrlRootNode
        {
            get
            {
                return thisXbrlRootNode;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<Context> Contexts
        {
            get
            {
                return thisContexts;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<XbrlSchema> Schemas
        {
            get
            {
                return thisSchemas;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<Fact> Facts
        {
            get
            {
                return thisFacts;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<Unit> Units
        {
            get
            {
                return thisUnits;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public List<FootnoteLink> FootnoteLinks
        {
            get
            {
                return thisFootnoteLinks;
            }
        }

        //===============================================================================
        #endregion
        //===============================================================================

        //===============================================================================
        #region Constructors
        //===============================================================================

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal XbrlFragment(XbrlDocument ParentDocument, XmlNode XbrlRootNode)
        {
            thisDocument = ParentDocument;
            thisXbrlRootNode = XbrlRootNode;
            CreateNamespaceManager();
            //---------------------------------------------------------------------------
            // Load.
            //---------------------------------------------------------------------------
            ReadTaxonomySchemaRefs();
            ReadContexts();
            ReadUnits();
            ReadFacts();
            ReadFootnoteLinks();
            if (Loaded != null)
                Loaded(this, null);
            //---------------------------------------------------------------------------
            // Validate.
            //---------------------------------------------------------------------------
            ValidateContextRefs();
            ValidateUnitRefs();
            ValidateContextTimeSpansAgainstPeriodTypes();
            ValidateFootnoteLocations();
            ValidateFootnoteArcs();
            ValidateFacts();
            if (Validated != null)
                Validated(this, null);
        }

        //===============================================================================
        #endregion
        //===============================================================================

        //===============================================================================
        #region Public Methods
        //===============================================================================

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public Context GetContext(string ContextId)
        {
            foreach (Context CurrentContext in thisContexts)
            {
                if (CurrentContext.Id == ContextId)
                    return CurrentContext;
            }
            return null;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public Unit GetUnit(string UnitId)
        {
            foreach (Unit CurrentUnit in thisUnits)
            {
                if (CurrentUnit.Id == UnitId)
                    return CurrentUnit;
            }
            return null;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public Fact GetFact(string FactId)
        {
            foreach (Fact CurrentFact in thisFacts)
            {
                if (CurrentFact.Id == FactId)
                    return CurrentFact;
            }
            return null;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string GetUriForPrefix(string Prefix)
        {
            return thisUriPrefixDictionary.GetUriForPrefix(Prefix);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string GetPrefixForUri(string Uri)
        {
            return thisUriPrefixDictionary.GetPrefixForUri(Uri);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public XbrlSchema GetXbrlSchemaForPrefix(string Prefix)
        {
            string Uri = GetUriForPrefix(Prefix);
            if (Uri == null)
                return null;
            if (Uri.Length == 0)
                return null;
            foreach (XbrlSchema CurrentSchema in thisSchemas)
            {
                if (CurrentSchema.TargetNamespace.Equals(Uri) == true)
                    return CurrentSchema;
            }
            return null;
        }

        //===============================================================================
        #endregion
        //===============================================================================

        //===============================================================================
        #region Private Methods
        //===============================================================================

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadTaxonomySchemaRefs()
        {
            thisSchemas = new List<XbrlSchema>();
            string LinkbaseNamespacePrefix = thisUriPrefixDictionary.GetPrefixForUri("http://www.xbrl.org/2003/linkbase");
            StringBuilder XPathExpressionBuilder = new StringBuilder();
            XPathExpressionBuilder.AppendFormat("//{0}:schemaRef", LinkbaseNamespacePrefix);
            string XPathExpression = XPathExpressionBuilder.ToString();
            XmlNodeList SchemaRefNodes = thisXbrlRootNode.SelectNodes(XPathExpression, thisNamespaceManager);
            foreach (XmlNode SchemaRefNode in SchemaRefNodes)
                ReadTaxonomySchemaRef(SchemaRefNode);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadTaxonomySchemaRef(XmlNode SchemaRefNode)
        {
            StringBuilder HrefAttributeBuilder = new StringBuilder();
            string XlinkNamespacePrefix = thisUriPrefixDictionary.GetPrefixForUri("http://www.w3.org/1999/xlink");
            HrefAttributeBuilder.AppendFormat("{0}:href", XlinkNamespacePrefix);
            string HrefAttributeKey = HrefAttributeBuilder.ToString();
            string HrefAttributeValue = SchemaRefNode.Attributes[HrefAttributeKey].Value;
            thisSchemas.Add(new XbrlSchema(this, HrefAttributeValue));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void CreateNamespaceManager()
        {
            thisNamespaceManager = new XmlNamespaceManager(new NameTable());
            thisNamespaceManager.AddNamespace("instance", thisXbrlRootNode.NamespaceURI);
            foreach (XmlAttribute CurrentAttribute in thisXbrlRootNode.Attributes)
            {
                if (CurrentAttribute.Prefix == "xmlns")
                    thisNamespaceManager.AddNamespace(CurrentAttribute.LocalName, CurrentAttribute.Value);
            }
            thisUriPrefixDictionary = new UriPrefixDictionary();
            thisUriPrefixDictionary.Load(thisXbrlRootNode);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateContextRefs()
        {
            foreach (Fact CurrentFact in thisFacts)
            {
                string ContextRefValue = CurrentFact.ContextRefName;
                bool ContextFound = false;
                Context MatchingContext = null;
                foreach (Context CurrentContext in thisContexts)
                {
                    if (CurrentContext.Id == ContextRefValue)
                    {
                        ContextFound = true;
                        MatchingContext = CurrentContext;
                        CurrentFact.ContextRef = MatchingContext;
                    }
                }
                if (ContextFound == false)
                {
                    string MessageFormat = AssemblyResources.GetName("CannotFindContextForContextRef");
                    StringBuilder MessageBuilder = new StringBuilder();
                    MessageBuilder.AppendFormat(MessageFormat, ContextRefValue);
                    throw new XbrlException(MessageBuilder.ToString());
                }
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateUnitRefs()
        {
            foreach (Fact CurrentFact in thisFacts)
            {
                string UnitRefValue = CurrentFact.UnitRefName;
                bool UnitFound = false;
                Unit MatchingUnit = null;
                foreach (Unit CurrentUnit in thisUnits)
                {
                    if (CurrentUnit.Id == UnitRefValue)
                    {
                        UnitFound = true;
                        MatchingUnit = CurrentUnit;
                        CurrentFact.UnitRef = MatchingUnit;
                    }
                }
                if (UnitFound == false)
                {
                    string MessageFormat = AssemblyResources.GetName("CannotFindUnitForUnitRef");
                    StringBuilder MessageBuilder = new StringBuilder();
                    MessageBuilder.AppendFormat(MessageFormat, UnitRefValue);
                    throw new XbrlException(MessageBuilder.ToString());
                }
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadContexts()
        {
            thisContexts = new List<Context>();
            XmlNodeList ContextNodes = thisXbrlRootNode.SelectNodes("//instance:context", thisNamespaceManager);
            foreach (XmlNode ContextNode in ContextNodes)
                thisContexts.Add(new Context(this, ContextNode));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadFacts()
        {
            thisFacts = new List<Fact>();
            foreach (XmlNode CurrentChild in thisXbrlRootNode.ChildNodes)
            {
                if (IsTaxonomyNamespace(CurrentChild.NamespaceURI) == true)
                    thisFacts.Add(new Fact(this, CurrentChild));
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private bool IsTaxonomyNamespace(string CandidateNamespace)
        {
            foreach (XbrlSchema CurrentSchema in this.Schemas)
            {
                if (CandidateNamespace == CurrentSchema.TargetNamespace)
                    return true;
            }
            return false;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateContextTimeSpansAgainstPeriodTypes()
        {
            foreach (Fact CurrentFact in thisFacts)
            {
                switch (CurrentFact.SchemaElement.PeriodType)
                {
                    case Element.ElementPeriodType.Duration:
                        if (CurrentFact.ContextRef.DurationPeriod == false)
                        {
                            StringBuilder MessageBuilder = new StringBuilder();
                            string StringFormat = AssemblyResources.GetName("ElementSchemaDefinesDurationButUsedWithNonDurationContext");
                            MessageBuilder.AppendFormat(StringFormat, CurrentFact.SchemaElement.Schema.Path, CurrentFact.Name, CurrentFact.ContextRef.Id);
                            throw new XbrlException(MessageBuilder.ToString());
                        }
                        break;
                    case Element.ElementPeriodType.Instant:
                        if (CurrentFact.ContextRef.InstantPeriod == false)
                        {
                            StringBuilder MessageBuilder = new StringBuilder();
                            string StringFormat = AssemblyResources.GetName("ElementSchemaDefinesInstantButUsedWithNonInstantContext");
                            MessageBuilder.AppendFormat(StringFormat, CurrentFact.SchemaElement.Schema.Path, CurrentFact.Name, CurrentFact.ContextRef.Id);
                            throw new XbrlException(MessageBuilder.ToString());
                        }
                        break;
                }
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadUnits()
        {
            thisUnits = new List<Unit>();
            XmlNodeList UnitNodes = thisXbrlRootNode.SelectNodes("//instance:unit", thisNamespaceManager);
            foreach (XmlNode UnitNode in UnitNodes)
                thisUnits.Add(new Unit(UnitNode));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadFootnoteLinks()
        {
            thisFootnoteLinks = new List<FootnoteLink>();
            string LinkbaseNamespacePrefix = thisUriPrefixDictionary.GetPrefixForUri("http://www.xbrl.org/2003/linkbase");
            StringBuilder XPathExpressionBuilder = new StringBuilder();
            XPathExpressionBuilder.AppendFormat("//{0}:footnoteLink", LinkbaseNamespacePrefix);
            string XPathExpression = XPathExpressionBuilder.ToString();
            XmlNodeList FootnoteLinkNodes = thisXbrlRootNode.SelectNodes(XPathExpression, thisNamespaceManager);
            foreach (XmlNode FootnoteLinkNode in FootnoteLinkNodes)
                thisFootnoteLinks.Add(new FootnoteLink(FootnoteLinkNode));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateFootnoteArcs()
        {
            foreach (FootnoteLink CurrentFootnoteLink in thisFootnoteLinks)
            {
                foreach (FootnoteArc CurrentArc in CurrentFootnoteLink.FootnoteArcs)
                    ValidateFootnoteArc(CurrentArc);
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateFacts()
        {
            foreach (Fact CurrentFact in thisFacts)
                CurrentFact.Validate();
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateFootnoteArc(FootnoteArc CurrentArc)
        {
            FootnoteLocator Locator = CurrentArc.Link.GetLocator(CurrentArc.FromId);
            if (Locator == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("CannotFindFootnoteLocator");
                MessageBuilder.AppendFormat(StringFormat, CurrentArc.Title, CurrentArc.FromId);
                throw new XbrlException(MessageBuilder.ToString());
            }
            if ((Locator.Href.UrlSpecified == true) && (UrlReferencesFragmentDocument(Locator.Href) == false))
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("FootnoteReferencesFactInExternalDoc");
                MessageBuilder.AppendFormat(StringFormat, Locator.Href.ElementId, Locator.Href.Url);
                throw new XbrlException(MessageBuilder.ToString());
            }
            CurrentArc.From = GetFact(Locator.Href.ElementId);
            if (CurrentArc.From == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("CannotFindFactForFootnoteArc");
                MessageBuilder.AppendFormat(StringFormat, CurrentArc.Title, Locator.Href);
                throw new XbrlException(MessageBuilder.ToString());
            }
            CurrentArc.To = CurrentArc.Link.GetFootnote(CurrentArc.ToId);
            if (CurrentArc.To == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("CannotFindFootnoteForFootnoteArc");
                MessageBuilder.AppendFormat(StringFormat, CurrentArc.Title, CurrentArc.ToId);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateFootnoteLocations()
        {
            foreach (FootnoteLink CurrentFootnoteLink in thisFootnoteLinks)
            {
                foreach (FootnoteLocator CurrentLocation in CurrentFootnoteLink.FootnoteLocators)
                    ValidateFootnoteLocation(CurrentLocation.Href.ElementId);   // TODO
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateFootnoteLocation(string FootnoteLocationReference)
        {
            HyperlinkReference Reference = new HyperlinkReference(FootnoteLocationReference);
            if (Reference.UrlSpecified == true)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("FootnoteReferencesFactInExternalDoc");
                MessageBuilder.AppendFormat(StringFormat, Reference.ElementId, Reference.Url);
                throw new XbrlException(MessageBuilder.ToString());
            }
            if (GetFact(Reference.ElementId) == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("NoFactForFootnoteReference");
                MessageBuilder.AppendFormat(StringFormat, FootnoteLocationReference);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private bool UrlReferencesFragmentDocument(HyperlinkReference Href)
        {
            if (Href.UrlSpecified == false)
                return false;
            string DocFullPath = Path.GetFullPath(thisDocument.Filename);
            string HrefFullPath;
            if (Href.Url.IndexOf(Path.DirectorySeparatorChar) == -1)
                HrefFullPath = thisDocument.Path + Path.DirectorySeparatorChar + Href.Url;
            else
                HrefFullPath = Href.Url;
            if (DocFullPath.Equals(HrefFullPath) == true)
                return true;
            return false;
        }

        //===============================================================================
        #endregion
        //===============================================================================
    }
}

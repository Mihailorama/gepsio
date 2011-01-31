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
            return thisNamespaceManager.LookupNamespace(Prefix);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string GetPrefixForUri(string Uri)
        {
            return thisNamespaceManager.LookupPrefix(Uri);
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
            string LinkbaseNamespacePrefix = thisNamespaceManager.LookupPrefix("http://www.xbrl.org/2003/linkbase");
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
            string HrefAttributeValue = XmlUtilities.GetAttributeValue(SchemaRefNode, "http://www.w3.org/1999/xlink", "href");
            string Base = XmlUtilities.GetAttributeValue(SchemaRefNode, "http://www.w3.org/XML/1998/namespace", "base");
            thisSchemas.Add(new XbrlSchema(this, HrefAttributeValue, Base));
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
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateContextRefs()
        {
            foreach (Fact CurrentFact in thisFacts)
                ValidateContextRef(CurrentFact);
        }

        //-------------------------------------------------------------------------------
        // Validates the context reference for the given fact. Ensures that the context
        // ref can be tied to a defined context.
        //-------------------------------------------------------------------------------
        private void ValidateContextRef(Fact FactToValidate)
        {
            string ContextRefValue = FactToValidate.ContextRefName;
            if (ContextRefValue.Length == 0)
                return;

            bool ContextFound = false;
            Context MatchingContext = null;
            foreach (Context CurrentContext in thisContexts)
            {
                if (CurrentContext.Id == ContextRefValue)
                {
                    ContextFound = true;
                    MatchingContext = CurrentContext;
                    FactToValidate.ContextRef = MatchingContext;
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

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateUnitRefs()
        {
            foreach (Fact CurrentFact in thisFacts)
                ValidateUnitRef(CurrentFact);
        }

        //-------------------------------------------------------------------------------
        // Validates the unit reference for the given fact. Ensures that the unit ref
        // can be tied to a defined unit.
        //-------------------------------------------------------------------------------
        private void ValidateUnitRef(Fact FactToValidate)
        {
            string UnitRefValue = FactToValidate.UnitRefName;
            //-----------------------------------------------------------------------
            // According to section 4.6.2, non-numeric items must not have a unit
            // reference. So, if the fact's unit reference is blank, and this is a
            // non-numeric item, then there is nothing to validate.
            //-----------------------------------------------------------------------
            if (UnitRefValue.Length == 0)
            {
                if (FactToValidate.SchemaElement == null)
                    return;
                if (FactToValidate.Type == null)
                    return;
                if (FactToValidate.Type.IsNumeric() == false)
                    return;
            }
            //-----------------------------------------------------------------------
            // At this point, we have a unit ref should be matched to a unit.
            //-----------------------------------------------------------------------
            bool UnitFound = false;
            Unit MatchingUnit = null;
            foreach (Unit CurrentUnit in thisUnits)
            {
                if (CurrentUnit.Id == UnitRefValue)
                {
                    UnitFound = true;
                    MatchingUnit = CurrentUnit;
                    FactToValidate.UnitRef = MatchingUnit;
                }
            }
            //-----------------------------------------------------------------------
            // Check to see if a unit is found.
            //-----------------------------------------------------------------------
            if(UnitFound == false)
            {
                string MessageFormat = AssemblyResources.GetName("CannotFindUnitForUnitRef");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, UnitRefValue);
                throw new XbrlException(MessageBuilder.ToString());
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

        /// <summary>
        /// Reads all of the facts in the XBRL fragment and creates a Fact object for each.
        /// </summary>
        private void ReadFacts()
        {
            thisFacts = new List<Fact>();
            foreach (XmlNode CurrentChild in thisXbrlRootNode.ChildNodes)
            {
                if ((IsXbrlNamespace(CurrentChild.NamespaceURI) == false)
                    && (IsW3Namespace(CurrentChild.NamespaceURI) == false)
                    && (CurrentChild.NodeType != XmlNodeType.Comment))
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

        /// <summary>
        /// Determines whether or not a namespace URI is hosted by the www.xbrl.org domain.
        /// </summary>
        /// <param name="CandidateNamespace">A namespace URI.</param>
        /// <returns>True if the namespace URI is hosted by the www.xbrl.org domain; false otherwise.</returns>
        private bool IsXbrlNamespace(string CandidateNamespace)
        {
            CandidateNamespace = CandidateNamespace.Trim();
            if (CandidateNamespace.Length == 0)
                return false; Uri NamespaceUri = new Uri(CandidateNamespace);
            return NamespaceUri.Host.ToLower().Equals("www.xbrl.org");
        }

        /// <summary>
        /// Determines whether or not a namespace URI is hosted by the www.w3.org domain.
        /// </summary>
        /// <param name="CandidateNamespace">A namespace URI.</param>
        /// <returns>True if the namespace URI is hosted by the www.w3.org domain; false otherwise.</returns>
        private bool IsW3Namespace(string CandidateNamespace)
        {
            CandidateNamespace = CandidateNamespace.Trim();
            if (CandidateNamespace.Length == 0)
                return false;
            Uri NamespaceUri = new Uri(CandidateNamespace);
            return NamespaceUri.Host.ToLower().Equals("www.w3.org");
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
            string LinkbaseNamespacePrefix = thisNamespaceManager.LookupPrefix("http://www.xbrl.org/2003/linkbase");
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
        // Validate all of the facts found in the fragment. Multiple activities happen
        // here:
        //
        // * each fact is validated against its data type described in its definition
        //   via an <element> tag in a taxonomy schema
        // * any facts that participate in an arc role are checked
        //-------------------------------------------------------------------------------
        private void ValidateFacts()
        {
            foreach (Fact CurrentFact in thisFacts)
                CurrentFact.Validate();
            ValidateFactsReferencedInDefinitionArcRoles();
            ValidateSummationConcepts();
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private bool UrlReferencesFragmentDocument(HyperlinkReference Href)
        {
            if (Href.UrlSpecified == false)
                return false;
            string DocFullPath = Path.GetFullPath(thisDocument.Filename);
            string HrefFullPathString;
            if (Href.Url.IndexOf(Path.DirectorySeparatorChar) == -1)
                HrefFullPathString = thisDocument.Path + Path.DirectorySeparatorChar + Href.Url;
            else
                HrefFullPathString = Href.Url;
            string HrefFullPath = Path.GetFullPath(HrefFullPathString);
            if (DocFullPath.Equals(HrefFullPath) == true)
                return true;
            return false;
        }

        //===============================================================================
        #endregion
        //===============================================================================

        //===============================================================================
        #region Definition Arc Role Validation
        //===============================================================================

        //-------------------------------------------------------------------------------
        // Searches the associated XBRL schemas, looking for facts that are referenced
        // in arc roles.
        //-------------------------------------------------------------------------------
        private void ValidateFactsReferencedInDefinitionArcRoles()
        {
            foreach (XbrlSchema CurrentSchema in thisSchemas)
                ValidateFactsReferencedInDefinitionArcRoles(CurrentSchema);
        }

        //-------------------------------------------------------------------------------
        // Searches the given XBRL schemas, looking for facts that are referenced
        // in arc roles.
        //-------------------------------------------------------------------------------
        private void ValidateFactsReferencedInDefinitionArcRoles(XbrlSchema CurrentSchema)
        {
            foreach (LinkbaseDocument CurrentLinkbaseDocument in CurrentSchema.LinkbaseDocuments)
                ValidateFactsReferencedInDefinitionArcRoles(CurrentLinkbaseDocument);
        }

        //-------------------------------------------------------------------------------
        // Searches the given linkbase document, looking for facts that are referenced
        // in arc roles.
        //-------------------------------------------------------------------------------
        private void ValidateFactsReferencedInDefinitionArcRoles(LinkbaseDocument CurrentLinkbaseDocument)
        {
            foreach (DefinitionLink CurrentDefinitionLink in CurrentLinkbaseDocument.DefinitionLinks)
                ValidateFactsReferencedInDefinitionArcRoles(CurrentDefinitionLink);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateFactsReferencedInDefinitionArcRoles(DefinitionLink CurrentDefinitionLink)
        {
            foreach (DefinitionArc CurrentDefinitionArc in CurrentDefinitionLink.DefinitionArcs)
            {
                switch (CurrentDefinitionArc.Role)
                {
                    case DefinitionArc.RoleEnum.EssenceAlias:
                        ValidateEssenceAliasedFacts(CurrentDefinitionArc);
                        break;
                    case DefinitionArc.RoleEnum.RequiresElement:
                        ValidateRequiresElementFacts(CurrentDefinitionArc);
                        break;
                    default:
                        break;
                }
            }
        }

        //-------------------------------------------------------------------------------
        // Validate the "requires element" connection between two facts referenced in a
        // definition arc.
        //-------------------------------------------------------------------------------
        private void ValidateRequiresElementFacts(DefinitionArc RequiresElementDefinitionArc)
        {
            Locator CurrentFromLocator = RequiresElementDefinitionArc.FromLocator;
            Locator CurrentToLocator = RequiresElementDefinitionArc.ToLocator;
            int FromFactCount = CountFactInstances(CurrentFromLocator.HrefResourceId);
            int ToFactCount = CountFactInstances(CurrentToLocator.HrefResourceId);
            if (FromFactCount > ToFactCount)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("NotEnoughToFactsInRequiresElementRelationship");
                MessageBuilder.AppendFormat(StringFormat, CurrentFromLocator.HrefResourceId, CurrentToLocator.HrefResourceId);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }

        //-------------------------------------------------------------------------------
        // Returns a count of the number of facts with the given name.
        //-------------------------------------------------------------------------------
        private int CountFactInstances(string FactName)
        {
            int Count = 0;

            foreach (Fact CurrentFact in thisFacts)
            {
                if (CurrentFact.Name.Equals(FactName) == true)
                    Count++;
            }
            return Count;
        }

        //-------------------------------------------------------------------------------
        // Validate the essence alias between two facts referenced in a definition arc.
        //-------------------------------------------------------------------------------
        private void ValidateEssenceAliasedFacts(DefinitionArc EssenceAliasDefinitionArc)
        {
            Locator CurrentFromLocator = EssenceAliasDefinitionArc.FromLocator;
            Locator CurrentToLocator = EssenceAliasDefinitionArc.ToLocator;

            foreach (Fact CurrentFact in thisFacts)
            {
                try
                {
                    if (CurrentFact.Name.Equals(CurrentFromLocator.HrefResourceId) == true)
                        ValidateEssenceAliasedFacts(CurrentFact, CurrentToLocator.HrefResourceId);
                }
                catch (NullReferenceException nre)
                {
                }
            }
        }

        //-------------------------------------------------------------------------------
        // Validate the essence alias between a given fact and all other facts with the
        // given fact name.
        //-------------------------------------------------------------------------------
        private void ValidateEssenceAliasedFacts(Fact FromFact, string ToFactName)
        {
            foreach(Fact CurrentFact in thisFacts)
            {
                if (CurrentFact.Name.Equals(ToFactName) == true)
                    ValidateEssenceAliasedFacts(FromFact, CurrentFact);
            }
        }

        //-------------------------------------------------------------------------------
        // Validate the essence alias between two given facts.
        //-------------------------------------------------------------------------------
        private void ValidateEssenceAliasedFacts(Fact FromFact, Fact ToFact)
        {
            if (FromFact.ContextEquals(ToFact) == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("EssenceAliasFactsNotContextEquals");
                MessageBuilder.AppendFormat(StringFormat, FromFact.Name, ToFact.Name, FromFact.Id, ToFact.Id);
                throw new XbrlException(MessageBuilder.ToString());
            }
            if (FromFact.ParentEquals(ToFact) == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("EssenceAliasFactsNotParentEquals");
                MessageBuilder.AppendFormat(StringFormat, FromFact.Name, ToFact.Name, FromFact.Id, ToFact.Id);
                throw new XbrlException(MessageBuilder.ToString());
            }
            if (FromFact.UnitEquals(ToFact) == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("EssenceAliasFactsNotUnitEquals");
                MessageBuilder.AppendFormat(StringFormat, FromFact.Name, ToFact.Name, FromFact.Id, ToFact.Id);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }

        //===============================================================================
        #endregion
        //===============================================================================

        //===============================================================================
        #region Footnote Validation
        //===============================================================================

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

        //===============================================================================
        #endregion
        //===============================================================================

        //===============================================================================
        #region Summation Concept Validation
        //===============================================================================

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateSummationConcepts()
        {
            foreach (XbrlSchema CurrentSchema in thisSchemas)
                ValidateSummationConcepts(CurrentSchema);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateSummationConcepts(XbrlSchema CurrentSchema)
        {
            foreach (LinkbaseDocument CurrentLinkbaseDocument in CurrentSchema.LinkbaseDocuments)
                ValidateSummationConcepts(CurrentLinkbaseDocument);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateSummationConcepts(LinkbaseDocument CurrentLinkbaseDocument)
        {
            foreach (CalculationLink CurrentCalculationLink in CurrentLinkbaseDocument.CalculationLinks)
                ValidateSummationConcepts(CurrentCalculationLink);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateSummationConcepts(CalculationLink CurrentCalculationLink)
        {
            foreach (SummationConcept CurrentSummationConcept in CurrentCalculationLink.SummationConcepts)
                ValidateSummationConcept(CurrentCalculationLink, CurrentSummationConcept);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateSummationConcept(CalculationLink CurrentCalculationLink, SummationConcept CurrentSummationConcept)
        {
            Element SummationConceptElement = LocateElement(CurrentSummationConcept.SummationConceptLocator);
            Fact SummationConceptFact = LocateFact(SummationConceptElement);
            //---------------------------------------------------------------------------
            // If the summation concept fact doesn't exist, then there is no calculation
            // to perform.
            //---------------------------------------------------------------------------
            if (SummationConceptFact == null)
                //throw new XbrlException(AssemblyResources.BuildMessage("CannotFindFactForElement", SummationConceptElement.Id));
                return;
            double SummationConceptRoundedValue = SummationConceptFact.RoundedValue;
            double ContributingConceptRoundedValueTotal = 0;
            foreach (Locator CurrentLocator in CurrentSummationConcept.ContributingConceptLocators)
            {
                CalculationArc ContributingConceptCalculationArc = CurrentCalculationLink.GetCalculationArc(CurrentLocator);
                Element ContributingConceptElement = LocateElement(CurrentLocator);
                Fact ContributingConceptFact = LocateFact(ContributingConceptElement);
                if (ContributingConceptFact != null)
                {
                    double ContributingConceptRoundedValue = ContributingConceptFact.RoundedValue;
                    if (ContributingConceptCalculationArc.Weight != (decimal)(1.0))
                        ContributingConceptRoundedValue = ContributingConceptRoundedValue * (double)(ContributingConceptCalculationArc.Weight);
                    ContributingConceptRoundedValueTotal += ContributingConceptRoundedValue;
                }
            }
            ContributingConceptRoundedValueTotal = SummationConceptFact.Round(ContributingConceptRoundedValueTotal);
            if (SummationConceptRoundedValue != ContributingConceptRoundedValueTotal)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("SummationConceptError");
                MessageBuilder.AppendFormat(StringFormat, SummationConceptFact.Name, SummationConceptRoundedValue, ContributingConceptRoundedValueTotal);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }

        //===============================================================================
        #endregion
        //===============================================================================

        //===============================================================================
        #region Location Support
        //===============================================================================

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private Fact LocateFact(Locator FactLocator)
        {
            if (FactLocator == null)
                return null;
            foreach (Fact CurrentFact in thisFacts)
            {
                if (CurrentFact.Name.Equals(FactLocator.HrefResourceId) == true)
                    return CurrentFact;
            }
            return null;
        }


        /// <summary>
        /// Locates an element given an element locator.
        /// </summary>
        /// <param name="ElementLocator">The locator specifying the element to find.</param>
        /// <returns>The element referenced by the locator; null if the element cannot be found.</returns>
        private Element LocateElement(Locator ElementLocator)
        {
            foreach (XbrlSchema CurrentSchema in thisSchemas)
            {
                var FoundElement = CurrentSchema.LocateElement(ElementLocator);
                if (FoundElement != null)
                    return FoundElement;
            }
            return null;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private Fact LocateFact(Element FactElement)
        {
            if (FactElement == null)
                return null;
            foreach (Fact CurrentFact in thisFacts)
            {
                if (CurrentFact.SchemaElement.Equals(FactElement) == true)
                    return CurrentFact;
            }
            return null;
        }

        //===============================================================================
        #endregion
        //===============================================================================

    }
}

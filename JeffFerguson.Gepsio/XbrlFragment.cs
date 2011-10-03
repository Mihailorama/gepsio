using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A fragment of XBRL data. A collection of fragments is available in the <see cref="XbrlDocument"/>
    /// class.
    /// </summary>
    public class XbrlFragment
    {
        #region Delegates
        
        /// <summary>
        /// The delegate used to handle events fired by the class.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// Event arguments.
        /// </param>
        public delegate void XbrlEventHandler(object sender, EventArgs e);

        #endregion

        #region Events
        
        /// <summary>
        /// Event fired after a document has been loaded.
        /// </summary>
        public event XbrlEventHandler Loaded;

        /// <summary>
        /// Event fired after all XBRL validation has been completed.
        /// </summary>
        public event XbrlEventHandler Validated;

        #endregion

        #region Fields
        
        private XbrlDocument thisDocument;
        private XmlNode thisXbrlRootNode;
        private List<Context> thisContexts;
        private XmlNamespaceManager thisNamespaceManager;
        private List<XbrlSchema> thisSchemas;
        private List<Fact> thisFacts;
        private List<Unit> thisUnits;
        private List<FootnoteLink> thisFootnoteLinks;

        #endregion

        #region Properties

        /// <summary>
        /// A reference to the <see cref="XbrlDocument"/> instance in which the fragment
        /// was contained.
        /// </summary>
        public XbrlDocument Document
        {
            get
            {
                return thisDocument;
            }
        }

        /// <summary>
        /// The root XML node for the XBRL fragment.
        /// </summary>
        public XmlNode XbrlRootNode
        {
            get
            {
                return thisXbrlRootNode;
            }
        }

        /// <summary>
        /// A collection of <see cref="Context"/> objects representing all contexts found in the fragment.
        /// </summary>
        public List<Context> Contexts
        {
            get
            {
                return thisContexts;
            }
        }

        /// <summary>
        /// A collection of <see cref="XbrlSchema"/> objects representing all schemas found in the fragment.
        /// </summary>
        public List<XbrlSchema> Schemas
        {
            get
            {
                return thisSchemas;
            }
        }

        /// <summary>
        /// A collection of <see cref="Fact"/> objects representing all facts found in the fragment.
        /// </summary>
        public List<Fact> Facts
        {
            get
            {
                return thisFacts;
            }
        }

        /// <summary>
        /// A collection of <see cref="Unit"/> objects representing all units found in the fragment.
        /// </summary>
        public List<Unit> Units
        {
            get
            {
                return thisUnits;
            }
        }

        /// <summary>
        /// A collection of <see cref="FootnoteLink"/> objects representing all footnote links
        /// found in the fragment.
        /// </summary>
        public List<FootnoteLink> FootnoteLinks
        {
            get
            {
                return thisFootnoteLinks;
            }
        }

        #endregion

        #region Constructors

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
            ReadElementInstances();
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
            ValidateItems();
            if (Validated != null)
                Validated(this, null);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a reference to the context having the supplied context ID.
        /// </summary>
        /// <param name="ContextId">
        /// The ID of the context to return.
        /// </param>
        /// <returns>
        /// A reference to the context having the supplied context ID.
        /// A null is returned if no contexts with the supplied context ID is available.
        /// </returns>
        public Context GetContext(string ContextId)
        {
            foreach (Context CurrentContext in thisContexts)
            {
                if (CurrentContext.Id == ContextId)
                    return CurrentContext;
            }
            return null;
        }

        /// <summary>
        /// Returns a reference to the unit having the supplied unit ID.
        /// </summary>
        /// <param name="UnitId">
        /// The ID of the unit to return.
        /// </param>
        /// <returns>
        /// A reference to the unit having the supplied unit ID.
        /// A null is returned if no units with the supplied unit ID is available.
        /// </returns>
        public Unit GetUnit(string UnitId)
        {
            foreach (Unit CurrentUnit in thisUnits)
            {
                if (CurrentUnit.Id == UnitId)
                    return CurrentUnit;
            }
            return null;
        }

        /// <summary>
        /// Returns a reference to the fact having the supplied fact ID.
        /// </summary>
        /// <param name="FactId">
        /// The ID of the fact to return.
        /// </param>
        /// <returns>
        /// A reference to the fact having the supplied fact ID.
        /// A null is returned if no facts with the supplied fact ID is available.
        /// </returns>
        public Item GetFact(string FactId)
        {
            foreach (Item CurrentFact in thisFacts)
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
            {
                if(CurrentFact is Item)
                    ValidateContextRef(CurrentFact as Item);
            }
        }

        //-------------------------------------------------------------------------------
        // Validates the context reference for the given fact. Ensures that the context
        // ref can be tied to a defined context.
        //-------------------------------------------------------------------------------
        private void ValidateContextRef(Item ItemToValidate)
        {
            string ContextRefValue = ItemToValidate.ContextRefName;
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
                    ItemToValidate.ContextRef = MatchingContext;
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
            {
                if(CurrentFact is Item)
                    ValidateUnitRef(CurrentFact as Item);
            }
        }

        //-------------------------------------------------------------------------------
        // Validates the unit reference for the given fact. Ensures that the unit ref
        // can be tied to a defined unit.
        //-------------------------------------------------------------------------------
        private void ValidateUnitRef(Item ItemToValidate)
        {
            string UnitRefValue = ItemToValidate.UnitRefName;
            //-----------------------------------------------------------------------
            // According to section 4.6.2, non-numeric items must not have a unit
            // reference. So, if the fact's unit reference is blank, and this is a
            // non-numeric item, then there is nothing to validate.
            //-----------------------------------------------------------------------
            if (UnitRefValue.Length == 0)
            {
                if (ItemToValidate.SchemaElement == null)
                    return;
                if (ItemToValidate.Type == null)
                    return;
                if (ItemToValidate.Type.IsNumeric() == false)
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
                    ItemToValidate.UnitRef = MatchingUnit;
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
        /// Reads all of the element instances in the XBRL fragment and creates an object for each.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Element instances can be any of the following:
        /// </para>
        /// <para>
        /// <list type="bullet">
        /// <item>
        /// an item, represented in an XBRL schema by an element with a substitution group of "item"
        /// and represented by an <see cref="Item"/> object
        /// </item>
        /// <item>
        /// a tuple, represented in an XBRL schema by an element with a substitution group of "tuple"
        /// and represented by an <see cref="Tuple"/> object
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        private void ReadElementInstances()
        {
            thisFacts = new List<Fact>();
            foreach (XmlNode CurrentChild in thisXbrlRootNode.ChildNodes)
            {
                var CurrentFact = Fact.Create(this, CurrentChild);
                if (CurrentFact != null)
                    thisFacts.Add(CurrentFact);
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
                if (CurrentFact is Item)
                {
                    var CurrentItem = CurrentFact as Item;
                    switch (CurrentItem.SchemaElement.PeriodType)
                    {
                        case Element.ElementPeriodType.Duration:
                            if (CurrentItem.ContextRef.DurationPeriod == false)
                            {
                                StringBuilder MessageBuilder = new StringBuilder();
                                string StringFormat = AssemblyResources.GetName("ElementSchemaDefinesDurationButUsedWithNonDurationContext");
                                MessageBuilder.AppendFormat(StringFormat, CurrentItem.SchemaElement.Schema.Path, CurrentItem.Name, CurrentItem.ContextRef.Id);
                                throw new XbrlException(MessageBuilder.ToString());
                            }
                            break;
                        case Element.ElementPeriodType.Instant:
                            if (CurrentItem.ContextRef.InstantPeriod == false)
                            {
                                StringBuilder MessageBuilder = new StringBuilder();
                                string StringFormat = AssemblyResources.GetName("ElementSchemaDefinesInstantButUsedWithNonInstantContext");
                                MessageBuilder.AppendFormat(StringFormat, CurrentItem.SchemaElement.Schema.Path, CurrentItem.Name, CurrentItem.ContextRef.Id);
                                throw new XbrlException(MessageBuilder.ToString());
                            }
                            break;
                    }
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
        private void ValidateItems()
        {
            foreach (Fact CurrentFact in thisFacts)
            {
                var CurrentItem = CurrentFact as Item;
                if(CurrentItem != null)
                    CurrentItem.Validate();
            }
            ValidateItemsReferencedInDefinitionArcRoles();
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
        private void ValidateItemsReferencedInDefinitionArcRoles()
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
            if (CurrentFromLocator == null)
                throw new NullReferenceException("FromLocator is NULL in ValidateEssenceAliasedFacts()");
            Locator CurrentToLocator = EssenceAliasDefinitionArc.ToLocator;

            foreach (Fact CurrentFact in thisFacts)
            {
                var CurrentItem = CurrentFact as Item;
                if (CurrentItem != null)
                {
                    if (CurrentFact.Name.Equals(CurrentFromLocator.HrefResourceId) == true)
                        ValidateEssenceAliasedFacts(CurrentItem, CurrentToLocator.HrefResourceId);
                }
            }
        }

        //-------------------------------------------------------------------------------
        // Validate the essence alias between a given fact and all other facts with the
        // given fact name.
        //-------------------------------------------------------------------------------
        private void ValidateEssenceAliasedFacts(Item FromItem, string ToItemName)
        {
            foreach (Fact CurrentFact in thisFacts)
            {
                var CurrentItem = CurrentFact as Item;
                if (CurrentItem != null)
                {
                    if (CurrentFact.Name.Equals(ToItemName) == true)
                        ValidateEssenceAliasedFacts(FromItem, CurrentItem);
                }
            }
        }

        //-------------------------------------------------------------------------------
        // Validate the essence alias between two given facts.
        //-------------------------------------------------------------------------------
        private void ValidateEssenceAliasedFacts(Item FromItem, Item ToItem)
        {
            if (FromItem.ContextEquals(ToItem) == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("EssenceAliasFactsNotContextEquals");
                MessageBuilder.AppendFormat(StringFormat, FromItem.Name, ToItem.Name, FromItem.Id, ToItem.Id);
                throw new XbrlException(MessageBuilder.ToString());
            }
            if (FromItem.ParentEquals(ToItem) == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("EssenceAliasFactsNotParentEquals");
                MessageBuilder.AppendFormat(StringFormat, FromItem.Name, ToItem.Name, FromItem.Id, ToItem.Id);
                throw new XbrlException(MessageBuilder.ToString());
            }
            if (FromItem.UnitEquals(ToItem) == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("EssenceAliasFactsNotUnitEquals");
                MessageBuilder.AppendFormat(StringFormat, FromItem.Name, ToItem.Name, FromItem.Id, ToItem.Id);
                throw new XbrlException(MessageBuilder.ToString());
            }
            if (FromItem.RoundedValue != ToItem.RoundedValue)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("EssenceAliasFactsHaveDifferentRoundedValues");
                MessageBuilder.AppendFormat(StringFormat, FromItem.Name, ToItem.Name, FromItem.Id, FromItem.RoundedValue.ToString(), ToItem.Id, ToItem.RoundedValue.ToString());
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
            Item SummationConceptFact = LocateItem(SummationConceptElement);
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
                Item ContributingConceptFact = LocateItem(ContributingConceptElement);
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
        private Item LocateFact(Locator FactLocator)
        {
            if (FactLocator == null)
                return null;
            foreach (Item CurrentFact in thisFacts)
            {
                if (CurrentFact.Name.Equals(FactLocator.HrefResourceId) == true)
                    return CurrentFact;
            }
            return null;
        }


        /// <summary>
        /// Locates an element given an element locator.
        /// </summary>
        /// <param name="ElementLocator">
        /// The locator specifying the element to find.
        /// </param>
        /// <returns>
        /// The element referenced by the locator; null if the element cannot be found.
        /// </returns>
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
        private Item LocateItem(Element ItemElement)
        {
            if (ItemElement == null)
                return null;
            foreach (Fact CurrentFact in thisFacts)
            {
                var CurrentItem = CurrentFact as Item;
                if (CurrentItem != null)
                {
                    if (CurrentItem.SchemaElement.Equals(ItemElement) == true)
                        return CurrentItem;
                }
            }
            return null;
        }

        //===============================================================================
        #endregion
        //===============================================================================

    }
}

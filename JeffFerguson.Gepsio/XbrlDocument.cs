using JeffFerguson.Gepsio.IoC;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An XML document containing one or more XBRL fragments.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An XBRL fragment is a fragment of XBRL data having an xbrl tag as its root. In the generic case,
    /// an XBRL document will have an xbrl tag as the root tag of the XML document, and, in this case,
    /// the entire XBRL document is one large XBRL fragment. However, section 4.1 of the XBRL 2.1 Specification
    /// makes provisions for multiple XBRL fragments to be stored in a single document:
    /// </para>
    /// <para>
    /// "If multiple 'data islands' of XBRL mark-up are included in a larger document, the xbrl element is
    /// the container for each [fragment]."
    /// </para>
    /// <para>
    /// Gepsio supports this notion by defining an XBRL document containing a collection of one or more
    /// XBRL fragments, as in the following code sample:
    /// </para>
    /// <code>
    /// var myDocument = new XbrlDocument();
    /// myDocument.Load("myxbrldoc.xml");
    /// foreach(var currentFragment in myDocument.XbrlFragments)
    /// {
    ///     // XBRL data is available from the "currentFragment" variable
    /// }
    /// </code>
    /// <para>
    /// In the vast majority of cases, an XBRL document will be an XML document with the xbrl tag at its
    /// root, and, as a result, the <see cref="XbrlDocument"/> uses to load the XBRL document will have
    /// a single <see cref="XbrlFragment"/> in the document's fragments container. Consider, however, the
    /// possibility of having more than one fragment in a document, in accordance of the text in section
    /// 4.1 of the XBRL 2.1 Specification.
    /// </para>
    /// </remarks>
    public class XbrlDocument
    {

        // namespace URIs

        internal static string XbrlNamespaceUri = "http://www.xbrl.org/2003/instance";
        internal static string XbrlLinkbaseNamespaceUri = "http://www.xbrl.org/2003/linkbase";
        internal static string XbrlDimensionsNamespaceUri = "http://xbrl.org/2005/xbrldt";
        internal static string XbrlEssenceAliasArcroleNamespaceUri = "http://www.xbrl.org/2003/arcrole/essence-alias";
        internal static string XbrlGeneralSpecialArcroleNamespaceUri = "http://www.xbrl.org/2003/arcrole/general-special";
        internal static string XbrlSimilarTuplesArcroleNamespaceUri = "http://www.xbrl.org/2003/arcrole/similar-tuples";
        internal static string XbrlRequiresElementArcroleNamespaceUri = "http://www.xbrl.org/2003/arcrole/requires-element";
        internal static string XbrlFactFootnoteArcroleNamespaceUri = "http://www.xbrl.org/2003/arcrole/fact-footnote";
        internal static string XbrlIso4217NamespaceUri = "http://www.xbrl.org/2003/iso4217";
        internal static string XmlNamespaceUri = "http://www.w3.org/XML/1998/namespace";

        // role URIs

        internal static string XbrlLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/label";
        internal static string XbrlTerseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/terseLabel";
        internal static string XbrlVerboseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/verboseLabel";
        internal static string XbrlPositiveLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/positiveLabel";
        internal static string XbrlPositiveTerseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/positiveTerseLabel";
        internal static string XbrlPositiveVerboseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/positiveVerboseLabel";
        internal static string XbrlNegativeLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/negativeLabel";
        internal static string XbrlNegativeTerseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/negativeTerseLabel";
        internal static string XbrlNegativeVerboseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/negativeVerboseLabel";
        internal static string XbrlZeroLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/zeroLabel";
        internal static string XbrlZeroTerseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/zeroTerseLabel";
        internal static string XbrlZeroVerboseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/zeroVerboseLabel";
        internal static string XbrlTotalLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/totalLabel";
        internal static string XbrlPeriodStartLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/periodStartLabel";
        internal static string XbrlPeriodEndLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/periodEndLabel";
        internal static string XbrlDocumentationRoleNamespaceUri = "http://www.xbrl.org/2003/role/documentation";
        internal static string XbrlDocumentationGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/definitionGuidance";
        internal static string XbrlDisclosureGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/disclosureGuidance";
        internal static string XbrlPresentationGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/presentationGuidance";
        internal static string XbrlMeasurementGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/measurementGuidance";
        internal static string XbrlCommentaryGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/commentaryGuidance";
        internal static string XbrlExampleGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/exampleGuidance";
        internal static string XbrlCalculationLinkbaseReferenceRoleNamespaceUri = "http://www.xbrl.org/2003/role/calculationLinkbaseRef";
        internal static string XbrlDefinitionLinkbaseReferenceRoleNamespaceUri = "http://www.xbrl.org/2003/role/definitionLinkbaseRef";
        internal static string XbrlLabelLinkbaseReferenceRoleNamespaceUri = "http://www.xbrl.org/2003/role/labelLinkbaseRef";
        internal static string XbrlLabelPresentationReferenceRoleNamespaceUri = "http://www.xbrl.org/2003/role/presentationLinkbaseRef";
        internal static string XbrlReferencePresentationReferenceRoleNamespaceUri = "http://www.xbrl.org/2003/role/referenceLinkbaseRef";

        // fields

        private List<XbrlFragment> thisXbrlFragments;
        private string thisFilename;
        private string thisPath;

        /// <summary>
        /// The name of the XML document used to contain the XBRL data.
        /// </summary>
        public string Filename
        {
            get
            {
                return thisFilename;
            }
        }


        /// <summary>
        /// The path to the XML document used to contain the XBRL data.
        /// </summary>
        public string Path
        {
            get
            {
                return thisPath;
            }
        }


        /// <summary>
        /// A collection of <see cref="XbrlFragment"/> objects that contain the document's
        /// XBRL data.
        /// </summary>
        public List<XbrlFragment> XbrlFragments
        {
            get
            {
                return thisXbrlFragments;
            }
        }

        /// <summary>
        /// Evaluates to true if the document contains no XBRL validation errors. Evaluates to
        /// false if the document contains at least one XBRL validation error.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (thisXbrlFragments == null)
                    return true;
                if (thisXbrlFragments.Count == 0)
                    return true;
                foreach (var currentFragment in thisXbrlFragments)
                {
                    if (currentFragment.IsValid == false)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// A collection of all validation errors found while validating the fragment.
        /// </summary>
        public List<ValidationError> ValidationErrors
        {
            get
            {
                if (thisXbrlFragments == null)
                    return null;
                if (thisXbrlFragments.Count == 0)
                    return null;
                if (thisXbrlFragments.Count == 1)
                    return thisXbrlFragments[0].ValidationErrors;
                var aggregatedValidationErrors = new List<ValidationError>();
                foreach (var currentFragment in thisXbrlFragments)
                {
                    aggregatedValidationErrors.AddRange(currentFragment.ValidationErrors);
                }
                return aggregatedValidationErrors;
            }
        }

        /// <summary>
        /// The constructor for the XbrlDocument class.
        /// </summary>
        public XbrlDocument()
        {
            thisXbrlFragments = new List<XbrlFragment>();
        }

        /// <summary>
        /// Loads an XBRL document containing XBRL data.
        /// </summary>
        /// <param name="Filename">
        /// The filename of the XML document to load.
        /// </param>
        public void Load(string Filename)
        {
            var SchemaValidXbrl = Container.Resolve<IDocument>();
            SchemaValidXbrl.Load(Filename);
            thisFilename = Filename;
            thisPath = System.IO.Path.GetDirectoryName(thisFilename);
            var NewNamespaceManager = Container.Resolve<INamespaceManager>();
            NewNamespaceManager.Document = SchemaValidXbrl;
            NewNamespaceManager.AddNamespace("instance", XbrlNamespaceUri);
            INodeList XbrlNodes = SchemaValidXbrl.SelectNodes("//instance:xbrl", NewNamespaceManager);
            foreach(INode XbrlNode in XbrlNodes)
                thisXbrlFragments.Add(new XbrlFragment(this, NewNamespaceManager, XbrlNode));
        }

        /// <summary>
        /// Finds the <see cref="RoleType"/> object having the given ID.
        /// </summary>
        /// <param name="RoleTypeId">
        /// The ID of the role type to find.
        /// </param>
        /// <returns>
        /// The <see cref="RoleType"/> object having the given ID, or null if no
        /// object can be found.
        /// </returns>
        public RoleType GetRoleType(string RoleTypeId)
        {
            foreach (var currentFragment in XbrlFragments)
            {
                var roleTypeCandidate = currentFragment.GetRoleType(RoleTypeId);
                if (roleTypeCandidate != null)
                    return roleTypeCandidate;
            }
            return null;
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
            foreach (var currentFragment in XbrlFragments)
            {
                var calculationLinkCandidate = currentFragment.GetCalculationLink(CalculationLinkRole);
                if (calculationLinkCandidate != null)
                    return calculationLinkCandidate;
            }
            return null;
        }
    }
}

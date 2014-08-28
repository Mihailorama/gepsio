using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Globalization;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "label" as defined in the http://www.xbrl.org/2003/linkbase namespace. 
    /// </summary>
    public class Label
    {
        /// <summary>
        /// A list of possible roles for a label.
        /// </summary>
        /// <remarks>
        /// See Table 8 in section 5.2.2.2.2 in the XBRL Specification for more information about label roles.
        /// </remarks>
        public enum RoleEnum
        {
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/label.
            /// </summary>
            Standard,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/terseLabel.
            /// </summary>
            Short,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/verboseLabel.
            /// </summary>
            Verbose,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/positiveLabel.
            /// </summary>
            StandardPositiveValue,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/positiveTerseLabel.
            /// </summary>
            ShortPositiveValue,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/positiveVerboseLabel.
            /// </summary>
            VerbosePositiveValue,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/negativeLabel.
            /// </summary>
            StandardNegativeValue,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/negativeTerseLabel.
            /// </summary>
            ShortNegativeValue,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/negativeVerboseLabel.
            /// </summary>
            VerboseNegativeValue,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/zeroLabel.
            /// </summary>
            StandardZeroValue,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/zeroTerseLabel.
            /// </summary>
            ShortZeroValue,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/zeroVerboseLabel.
            /// </summary>
            VerboseZeroValue,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/totalLabel.
            /// </summary>
            Total,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/periodStartLabel.
            /// </summary>
            PeriodStart,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/periodEndLabel.
            /// </summary>
            PeriodEnd,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/documentation.
            /// </summary>
            Documentation,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/definitionGuidance.
            /// </summary>
            DefinitionGuidance,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/disclosureGuidance.
            /// </summary>
            DisclosureGuidance,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/presentationGuidance.
            /// </summary>
            PresentationGuidance,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/measurementGuidance.
            /// </summary>
            MeasurementGuidance,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/commentaryGuidance.
            /// </summary>
            CommentaryGuidance,
            /// <summary>
            /// A label with a role URI of http://www.xbrl.org/2003/role/exampleGuidance.
            /// </summary>
            ExampleGuidance
        }

        /// <summary>
        /// The role of this label.
        /// </summary>
        public RoleEnum Role
        {
            get;
            private set;
        }

        /// <summary>
        /// The ID of this label.
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// The text of this label.
        /// </summary>
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// The culture of this label.
        /// </summary>
        public CultureInfo Culture
        {
            get;
            private set;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal Label(INode LabelNode)
        {
            this.Id = LabelNode.GetAttributeValue(XlinkNode.xlinkNamespace, "label");
            this.Text = LabelNode.InnerText;
            SetRole(LabelNode.GetAttributeValue(XlinkNode.xlinkNamespace, "role"));
            string LanguageValue = LabelNode.GetAttributeValue(XbrlDocument.XmlNamespaceUri, "lang");
            this.Culture = new CultureInfo(LanguageValue);
        }

        //------------------------------------------------------------------------------------
        // See Table 8 in section 5.2.2.2.2 in the XBRL Spec.
        //------------------------------------------------------------------------------------
        private void SetRole(string RoleUri)
        {
            this.Role = RoleEnum.Standard;
            if (RoleUri.Equals(XbrlDocument.XbrlLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.Standard;
            else if (RoleUri.Equals(XbrlDocument.XbrlTerseLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.Short;
            else if (RoleUri.Equals(XbrlDocument.XbrlVerboseLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.Verbose;
            else if (RoleUri.Equals(XbrlDocument.XbrlPositiveLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.StandardPositiveValue;
            else if (RoleUri.Equals(XbrlDocument.XbrlPositiveTerseLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.ShortPositiveValue;
            else if (RoleUri.Equals(XbrlDocument.XbrlPositiveVerboseLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.VerbosePositiveValue;
            else if (RoleUri.Equals(XbrlDocument.XbrlNegativeLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.StandardNegativeValue;
            else if (RoleUri.Equals(XbrlDocument.XbrlNegativeTerseLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.ShortNegativeValue;
            else if (RoleUri.Equals(XbrlDocument.XbrlNegativeVerboseLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.VerboseNegativeValue;
            else if (RoleUri.Equals(XbrlDocument.XbrlZeroLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.StandardZeroValue;
            else if (RoleUri.Equals(XbrlDocument.XbrlZeroTerseLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.ShortZeroValue;
            else if (RoleUri.Equals(XbrlDocument.XbrlZeroVerboseLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.VerboseZeroValue;
            else if (RoleUri.Equals(XbrlDocument.XbrlTotalLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.Total;
            else if (RoleUri.Equals(XbrlDocument.XbrlPeriodStartLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.PeriodStart;
            else if (RoleUri.Equals(XbrlDocument.XbrlPeriodEndLabelRoleNamespaceUri) == true)
                this.Role = RoleEnum.PeriodEnd;
            else if (RoleUri.Equals(XbrlDocument.XbrlDocumentationRoleNamespaceUri) == true)
                this.Role = RoleEnum.Documentation;
            else if (RoleUri.Equals(XbrlDocument.XbrlDocumentationGuidanceRoleNamespaceUri) == true)
                this.Role = RoleEnum.DefinitionGuidance;
            else if (RoleUri.Equals(XbrlDocument.XbrlDisclosureGuidanceRoleNamespaceUri) == true)
                this.Role = RoleEnum.DisclosureGuidance;
            else if (RoleUri.Equals(XbrlDocument.XbrlPresentationGuidanceRoleNamespaceUri) == true)
                this.Role = RoleEnum.PresentationGuidance;
            else if (RoleUri.Equals(XbrlDocument.XbrlMeasurementGuidanceRoleNamespaceUri) == true)
                this.Role = RoleEnum.MeasurementGuidance;
            else if (RoleUri.Equals(XbrlDocument.XbrlCommentaryGuidanceRoleNamespaceUri) == true)
                this.Role = RoleEnum.CommentaryGuidance;
            else if (RoleUri.Equals(XbrlDocument.XbrlExampleGuidanceRoleNamespaceUri) == true)
                this.Role = RoleEnum.ExampleGuidance;
        }
    }
}

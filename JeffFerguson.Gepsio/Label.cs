using System.Globalization;
using System.Xml;

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

        private CultureInfo thisCultureInfo;
        private string thisLabelText;

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
        internal Label(XmlNode LabelNode)
        {
            this.Id = XmlUtilities.GetAttributeValue(LabelNode, "http://www.w3.org/1999/xlink", "label");
            this.Text = LabelNode.InnerText;
            SetRole(XmlUtilities.GetAttributeValue(LabelNode, "http://www.w3.org/1999/xlink", "role"));
            string LanguageValue = XmlUtilities.GetAttributeValue(LabelNode, "http://www.w3.org/XML/1998/namespace", "lang");
            this.Culture = new CultureInfo(LanguageValue);
        }

        //------------------------------------------------------------------------------------
        // See Table 8 in section 5.2.2.2.2 in the XBRL Spec.
        //------------------------------------------------------------------------------------
        private void SetRole(string RoleUri)
        {
            this.Role = RoleEnum.Standard;
            if (RoleUri.Equals("http://www.xbrl.org/2003/role/label") == true)
                this.Role = RoleEnum.Standard;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/terseLabel") == true)
                this.Role = RoleEnum.Short;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/verboseLabel") == true)
                this.Role = RoleEnum.Verbose;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/positiveLabel") == true)
                this.Role = RoleEnum.StandardPositiveValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/positiveTerseLabel") == true)
                this.Role = RoleEnum.ShortPositiveValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/positiveVerboseLabel") == true)
                this.Role = RoleEnum.VerbosePositiveValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/negativeLabel") == true)
                this.Role = RoleEnum.StandardNegativeValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/negativeTerseLabel") == true)
                this.Role = RoleEnum.ShortNegativeValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/negativeVerboseLabel") == true)
                this.Role = RoleEnum.VerboseNegativeValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/zeroLabel") == true)
                this.Role = RoleEnum.StandardZeroValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/zeroTerseLabel") == true)
                this.Role = RoleEnum.ShortZeroValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/zeroVerboseLabel") == true)
                this.Role = RoleEnum.VerboseZeroValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/totalLabel") == true)
                this.Role = RoleEnum.Total;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/periodStartLabel") == true)
                this.Role = RoleEnum.PeriodStart;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/periodEndLabel") == true)
                this.Role = RoleEnum.PeriodEnd;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/documentation") == true)
                this.Role = RoleEnum.Documentation;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/definitionGuidance") == true)
                this.Role = RoleEnum.DefinitionGuidance;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/disclosureGuidance") == true)
                this.Role = RoleEnum.DisclosureGuidance;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/presentationGuidance") == true)
                this.Role = RoleEnum.PresentationGuidance;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/measurementGuidance") == true)
                this.Role = RoleEnum.MeasurementGuidance;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/commentaryGuidance") == true)
                this.Role = RoleEnum.CommentaryGuidance;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/exampleGuidance") == true)
                this.Role = RoleEnum.ExampleGuidance;
        }
    }
}

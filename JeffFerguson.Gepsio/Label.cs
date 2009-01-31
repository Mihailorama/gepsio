using System.Xml;
using System.Globalization;

namespace JeffFerguson.Gepsio
{
    public class Label
    {
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public enum RoleEnum
        {
            Standard,
            Short,
            Verbose,
            StandardPositiveValue,
            ShortPositiveValue,
            VerbosePositiveValue,
            StandardNegativeValue,
            ShortNegativeValue,
            VerboseNegativeValue,
            StandardZeroValue,
            ShortZeroValue,
            VerboseZeroValue,
            Total,
            PeriodStart,
            PeriodEnd,
            Documentation,
            DefinitionGuidance,
            DisclosureGuidance,
            PresentationGuidance,
            MeasurementGuidance,
            CommentaryGuidance,
            ExampleGuidance
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private RoleEnum thisRole;
        private CultureInfo thisCultureInfo;
        private string thisLabelId;
        private string thisLabelText;

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public RoleEnum Role
        {
            get
            {
                return thisRole;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string Id
        {
            get
            {
                return thisLabelId;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string Text
        {
            get
            {
                return thisLabelText;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public CultureInfo Culture
        {
            get
            {
                return thisCultureInfo;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal Label(XmlNode LabelNode)
        {
            thisLabelId = XmlUtilities.GetAttributeValue(LabelNode, "http://www.w3.org/1999/xlink", "label");
            thisLabelText = LabelNode.InnerText;
            SetRole(XmlUtilities.GetAttributeValue(LabelNode, "http://www.w3.org/1999/xlink", "role"));
            string LanguageValue = XmlUtilities.GetAttributeValue(LabelNode, "http://www.w3.org/XML/1998/namespace", "lang");
            thisCultureInfo = new CultureInfo(LanguageValue);
        }

        //------------------------------------------------------------------------------------
        // See Table 8 in section 5.2.2.2.2 in the XBRL Spec.
        //------------------------------------------------------------------------------------
        private void SetRole(string RoleUri)
        {
            thisRole = RoleEnum.Standard;
            if (RoleUri.Equals("http://www.xbrl.org/2003/role/label") == true)
                thisRole = RoleEnum.Standard;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/terseLabel") == true)
                thisRole = RoleEnum.Short;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/verboseLabel") == true)
                thisRole = RoleEnum.Verbose;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/positiveLabel") == true)
                thisRole = RoleEnum.StandardPositiveValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/positiveTerseLabel") == true)
                thisRole = RoleEnum.ShortPositiveValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/positiveVerboseLabel") == true)
                thisRole = RoleEnum.VerbosePositiveValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/negativeLabel") == true)
                thisRole = RoleEnum.StandardNegativeValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/negativeTerseLabel") == true)
                thisRole = RoleEnum.ShortNegativeValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/negativeVerboseLabel") == true)
                thisRole = RoleEnum.VerboseNegativeValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/zeroLabel") == true)
                thisRole = RoleEnum.StandardZeroValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/zeroTerseLabel") == true)
                thisRole = RoleEnum.ShortZeroValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/zeroVerboseLabel") == true)
                thisRole = RoleEnum.VerboseZeroValue;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/totalLabel") == true)
                thisRole = RoleEnum.Total;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/periodStartLabel") == true)
                thisRole = RoleEnum.PeriodStart;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/periodEndLabel") == true)
                thisRole = RoleEnum.PeriodEnd;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/documentation") == true)
                thisRole = RoleEnum.Documentation;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/definitionGuidance") == true)
                thisRole = RoleEnum.DefinitionGuidance;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/disclosureGuidance") == true)
                thisRole = RoleEnum.DisclosureGuidance;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/presentationGuidance") == true)
                thisRole = RoleEnum.PresentationGuidance;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/measurementGuidance") == true)
                thisRole = RoleEnum.MeasurementGuidance;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/commentaryGuidance") == true)
                thisRole = RoleEnum.CommentaryGuidance;
            else if (RoleUri.Equals("http://www.xbrl.org/2003/role/exampleGuidance") == true)
                thisRole = RoleEnum.ExampleGuidance;
        }
    }
}

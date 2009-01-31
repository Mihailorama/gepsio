using System.Xml;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// 
    /// </summary>
    public class Unit
    {
        private XmlNode thisUnitNode;
        private string thisId;
        private List<QualifiedName> thisMeasureQualifiedNames;
        private RegionInfo thisRegionInfo;
        private CultureInfo thisCultureInfo;
        private bool thisRatio;
        private List<QualifiedName> thisRatioNumeratorQualifiedNames;
        private List<QualifiedName> thisRatioDenominatorQualifiedNames;

        public string Id
        {
            get
            {
                return thisId;
            }
        }

        public List<QualifiedName> MeasureQualifiedNames
        {
            get
            {
                return thisMeasureQualifiedNames;
            }
        }

        public RegionInfo RegionInformation
        {
            get
            {
                return thisRegionInfo;
            }
        }

        public CultureInfo CultureInformation
        {
            get
            {
                return thisCultureInfo;
            }
        }

        public bool Ratio
        {
            get
            {
                return thisRatio;
            }
        }

        internal Unit(XmlNode UnitNode)
        {
            thisRegionInfo = null;
            thisUnitNode = UnitNode;
            thisId = thisUnitNode.Attributes["id"].Value;
            thisMeasureQualifiedNames = new List<QualifiedName>();
            thisRatio = false;
            thisRatioNumeratorQualifiedNames = new List<QualifiedName>();
            thisRatioDenominatorQualifiedNames = new List<QualifiedName>();
            foreach (XmlNode CurrentChild in UnitNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("measure") == true)
                    thisMeasureQualifiedNames.Add(new QualifiedName(CurrentChild));
                else if (CurrentChild.LocalName.Equals("divide") == true)
                {
                    ProcessDivideChildElement(CurrentChild);
                    CheckForMeasuresCommonToNumeratorsAndDenominators();
                    thisRatio = true;
                }
            }
        }

        private void CheckForMeasuresCommonToNumeratorsAndDenominators()
        {
            foreach (QualifiedName CurrentNumeratorMeasure in thisRatioNumeratorQualifiedNames)
            {
                if (thisRatioDenominatorQualifiedNames.Contains(CurrentNumeratorMeasure) == true)
                {
                    string MessageFormat = AssemblyResources.GetName("UnitRatioUsesSameMeasureInNumeratorAndDenominator");
                    StringBuilder MessageFormatBuilder = new StringBuilder();
                    MessageFormatBuilder.AppendFormat(MessageFormat, thisId, CurrentNumeratorMeasure.ToString());
                    throw new XbrlException(MessageFormatBuilder.ToString());
                }
            }
        }

        private void ProcessDivideChildElement(XmlNode UnitDivideNode)
        {
            foreach (XmlNode CurrentChild in UnitDivideNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("unitNumerator") == true)
                    ProcessUnitNumerators(CurrentChild);
                else if (CurrentChild.LocalName.Equals("unitDenominator") == true)
                    ProcessUnitDenominators(CurrentChild);
            }
        }

        private void ProcessUnitDenominators(XmlNode UnitDivideDenominatorNode)
        {
            foreach (XmlNode CurrentChild in UnitDivideDenominatorNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("measure") == true)
                    thisRatioDenominatorQualifiedNames.Add(new QualifiedName(CurrentChild));
            }
        }

        private void ProcessUnitNumerators(XmlNode UnitDivideNumeratorNode)
        {
            foreach (XmlNode CurrentChild in UnitDivideNumeratorNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("measure") == true)
                    thisRatioNumeratorQualifiedNames.Add(new QualifiedName(CurrentChild));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal void SetCultureAndRegionInfoFromISO4217Code(string Iso4217Code)
        {
            //--------------------------------------------------------------------------------
            // See if any obsolete ISO 4217 codes are being used and support those separately.
            //--------------------------------------------------------------------------------
            if (Iso4217Code.Equals("DEM") == true)
            {
                SetCultureAndRegionInfoFromRegionInfoName("DE");
                return;
            }
            //--------------------------------------------------------------------------------
            // Get a list of all cultures and find one whose region information specifies the
            // given ISO 4217 code as its currency symbol.
            //--------------------------------------------------------------------------------
            CultureInfo[] AllSpecificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CurrentCultureInfo in AllSpecificCultures)
            {
                RegionInfo CurrentRegionInfo = new RegionInfo(CurrentCultureInfo.LCID);
                if (CurrentRegionInfo.ISOCurrencySymbol == Iso4217Code)
                {
                    thisCultureInfo = CurrentCultureInfo;
                    thisRegionInfo = CurrentRegionInfo;
                    return;
                }
            }
        }

        //------------------------------------------------------------------------------------
        // This method is a bit of a hack so that Gepsio passes unit test 304.24 in the
        // XBRL-CONF-CR3-2007-03-05 conformance suite. The XBRL document in 304.24 uses a unit
        // of measure called iso4217:DEM, which is an obsolete ISO 4217 currency code for the
        // German Mark. This has been replaced in favor of the Euro.
        //
        // This method searches for appropriate CultureInfo and RegionInfo settings given the
        // name of a region.
        //------------------------------------------------------------------------------------
        private void SetCultureAndRegionInfoFromRegionInfoName(string RegionInfoName)
        {
            CultureInfo[] AllSpecificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CurrentCultureInfo in AllSpecificCultures)
            {
                RegionInfo CurrentRegionInfo = new RegionInfo(CurrentCultureInfo.LCID);
                if (CurrentRegionInfo.Name == RegionInfoName)
                {
                    thisCultureInfo = CurrentCultureInfo;
                    thisRegionInfo = CurrentRegionInfo;
                    return;
                }
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal bool StructureEquals(Unit OtherUnit)
        {
            if (thisUnitNode == null)
                return false;
            if (OtherUnit.thisUnitNode == null)
                return false;
            if (thisUnitNode.StructureEquals(OtherUnit.thisUnitNode) == false)
                return false;
            if (this.Ratio == false)
                return NonRatioStructureEquals(OtherUnit);
            else
                return RatioStructureEquals(OtherUnit);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private bool RatioStructureEquals(Unit OtherUnit)
        {
            if (QualifiedNameListsStructureEquals(thisRatioNumeratorQualifiedNames, OtherUnit.thisRatioNumeratorQualifiedNames) == false)
                return false;
            if (QualifiedNameListsStructureEquals(thisRatioDenominatorQualifiedNames, OtherUnit.thisRatioDenominatorQualifiedNames) == false)
                return false;
            return true;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private bool NonRatioStructureEquals(Unit OtherUnit)
        {
            return QualifiedNameListsStructureEquals(MeasureQualifiedNames, OtherUnit.MeasureQualifiedNames);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private bool QualifiedNameListsStructureEquals(List<QualifiedName> List1, List<QualifiedName> List2)
        {
            if (List1.Count != List2.Count)
                return false;
            foreach (QualifiedName CurrentQualifiedName in List1)
            {
                if (List2.Contains(CurrentQualifiedName) == false)
                    return false;
            }
            return true;
        }
    }
}

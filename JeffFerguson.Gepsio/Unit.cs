using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;

namespace JeffFerguson.Gepsio
{
    public class Unit
    {
        private XmlNode thisUnitNode;
        private string thisId;
        private QualifiedName thisMeasureQualifiedName;
        private RegionInfo thisRegionInfo;
        private CultureInfo thisCultureInfo;

        public string Id
        {
            get
            {
                return thisId;
            }
        }

        public QualifiedName MeasureQualifiedName
        {
            get
            {
                return thisMeasureQualifiedName;
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

        internal Unit(XmlNode UnitNode)
        {
            thisRegionInfo = null;
            thisUnitNode = UnitNode;
            thisId = thisUnitNode.Attributes["id"].Value;
            thisMeasureQualifiedName = null;
            foreach (XmlNode CurrentChild in UnitNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("measure") == true)
                    thisMeasureQualifiedName = new QualifiedName(CurrentChild);
            }
        }

        internal void SetCultureAndRegionInfoFromISO4217Code(string Iso4217Code)
        {
            CultureInfo[] AllSpecificCultures;
            
            AllSpecificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
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
    }
}

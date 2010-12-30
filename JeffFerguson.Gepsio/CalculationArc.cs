using System.Xml;
using System;

namespace JeffFerguson.Gepsio
{
    public class CalculationArc
    {
        //     <calculationArc xlink:type="arc" xlink:arcrole="http://www.xbrl.org/2003/arcrole/summation-item" xlink:from="calcinferprecisiontestcase_A" xlink:to="calcinferprecisiontestcase_C" order="2" weight="1" use="optional" />

        private string thisFromId;
        private string thisToId;
        private decimal thisOrder;
        private decimal thisWeight;
        private Locator thisFromLocator;
        private Locator thisToLocator;

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string FromId
        {
            get
            {
                return thisFromId;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string ToId
        {
            get
            {
                return thisToId;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public Locator FromLocator
        {
            get
            {
                return thisFromLocator;
            }
            internal set
            {
                thisFromLocator = value;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public Locator ToLocator
        {
            get
            {
                return thisToLocator;
            }
            internal set
            {
                thisToLocator = value;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public decimal Order
        {
            get
            {
                return thisOrder;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public decimal Weight
        {
            get
            {
                return thisWeight;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal CalculationArc(XmlNode CalculationArcNode)
        {
            thisFromId = XmlUtilities.GetAttributeValue(CalculationArcNode, "http://www.w3.org/1999/xlink", "from");
            thisToId = XmlUtilities.GetAttributeValue(CalculationArcNode, "http://www.w3.org/1999/xlink", "to");
            string OrderString = XmlUtilities.GetAttributeValue(CalculationArcNode, "order");
            thisOrder = Convert.ToDecimal(OrderString);
            string WeightString = XmlUtilities.GetAttributeValue(CalculationArcNode, "weight");
            thisWeight = Convert.ToDecimal(WeightString);
        }
    }
}

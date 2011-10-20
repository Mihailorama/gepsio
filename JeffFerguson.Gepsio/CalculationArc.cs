using System.Xml;
using System;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The CalculationArc manages information stored in a calculation arc. Calculation arcs are found in
    /// calculation linkbase documents.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Calculation arcs take the following form:
    /// </para>
    /// <para>
    /// <code>
    /// &lt;calculationArc
    /// xlink:type="arc"
    /// xlink:arcrole="http://www.xbrl.org/2003/arcrole/summation-item"
    /// xlink:from="FromLocatorLabel"
    /// xlink:to="ToLocatorLabel"
    /// order="[OrderValue]"
    /// weight="[WeightValue]"
    /// use="[UseValue]"
    /// /&gt;
    /// </code>
    /// </para>
    /// <para>
    /// It is important to note that the "to" label in a calculation arc may reference more than one locator. The 397.00 test in the XBRL-CONF-CR3-2007-03-05
    /// conformance suite uses the following calculation link in 397-ABC-calculation.xml:
    /// </para>
    /// <para>
    /// <code>
    /// &lt;calculationLink xlink:type="extended" xlink:role="http://www.xbrl.org/2003/role/link"&gt;
    ///     &lt;loc xlink:type="locator" xlink:href="397-ABC.xsd#A" xlink:label="summationItem" /&gt;
    ///     &lt;loc xlink:type="locator" xlink:href="397-ABC.xsd#B" xlink:label="contributingItem" /&gt;
    ///     &lt;loc xlink:type="locator" xlink:href="397-ABC.xsd#C" xlink:label="contributingItem" /&gt;
    ///     &lt;!-- A = B + C --&gt;
    ///     &lt;calculationArc xlink:type="arc" xlink:arcrole="http://www.xbrl.org/2003/arcrole/summation-item" xlink:from="summationItem" xlink:to="contributingItem" weight="1"/&gt;
    /// &lt;/calculationLink&gt;
    /// </code>
    /// </para>
    /// <para>
    /// Note that the calculation arc goes from a label of "summationItem" to a label of "contributingItem". Note also that there are two locators that match
    /// the "contributingItem" label: the locator with an href of "397-ABC.xsd#B" and a label with an href of "397-ABC.xsd#C". Both locators must be used for
    /// any calculations performed in satisfcation of the calculation arc.
    /// </para>
    /// </remarks>
    public class CalculationArc
    {
        private string thisFromId;
        private string thisToId;
        private decimal thisOrder;
        private decimal thisWeight;
        private Locator thisFromLocator;
        private List<Locator> thisToLocators;

        /// <summary>
        /// The ID of the "from" label referenced in the calculation arc.
        /// </summary>
        public string FromId
        {
            get
            {
                return thisFromId;
            }
        }

        /// <summary>
        /// The ID of the "to" label referenced in the calculation arc.
        /// </summary>
        public string ToId
        {
            get
            {
                return thisToId;
            }
        }

        /// <summary>
        /// The locator referenced by the "from" label referenced in the calculation arc.
        /// </summary>
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

        /// <summary>
        /// A collection of locators referenced by the "to" label referenced in the calculation arc.
        /// </summary>
        public List<Locator> ToLocators
        {
            get
            {
                return thisToLocators;
            }
        }

        /// <summary>
        /// The value of the "order" attribute used in the calculation arc.
        /// </summary>
        public decimal Order
        {
            get
            {
                return thisOrder;
            }
        }

        /// <summary>
        /// The value of the "weight" attribute used in the calculation arc.
        /// </summary>
        public decimal Weight
        {
            get
            {
                return thisWeight;
            }
        }

        /// <summary>
        /// The constructor for the CalculationArc class.
        /// </summary>
        /// <param name="CalculationArcNode">
        /// The XML node for the calculation arc.
        /// </param>
        internal CalculationArc(XmlNode CalculationArcNode)
        {
            thisToLocators = new List<Locator>();
            thisFromId = XmlUtilities.GetAttributeValue(CalculationArcNode, "http://www.w3.org/1999/xlink", "from");
            thisToId = XmlUtilities.GetAttributeValue(CalculationArcNode, "http://www.w3.org/1999/xlink", "to");
            string OrderString = XmlUtilities.GetAttributeValue(CalculationArcNode, "order");
            if(string.IsNullOrEmpty(OrderString) == false)
                thisOrder = Convert.ToDecimal(OrderString);
            string WeightString = XmlUtilities.GetAttributeValue(CalculationArcNode, "weight");
            if (string.IsNullOrEmpty(WeightString) == false)
                thisWeight = Convert.ToDecimal(WeightString);
            else
                thisWeight = (decimal)(1.0);
        }

        /// <summary>
        /// Adds a new locator to the arc's collection of "To" locators.
        /// </summary>
        /// <param name="ToLocator">
        /// The locator to be added to the "to" locator collection.
        /// </param>
        internal void AddToLocator(Locator ToLocator)
        {
            thisToLocators.Add(ToLocator);
        }
    }
}

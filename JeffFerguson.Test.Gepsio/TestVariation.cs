using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Test.Gepsio
{
    internal class TestVariation
    {
        string thisId;
        string thisName;
        string thisDescription;
        string thisXsd;
        string thisInstance;
        bool thisReadXsdFirst;
        bool thisReadInstanceFirst;
        bool thisValidityExpected;

        internal string Id
        {
            get
            {
                return thisId;
            }
        }

        internal string Name
        {
            get
            {
                return thisName;
            }
        }

        internal string Description
        {
            get
            {
                return thisDescription;
            }
        }

        internal string Xsd
        {
            get
            {
                return thisXsd;
            }
        }

        internal string Instance
        {
            get
            {
                return thisInstance;
            }
        }

        internal bool ReadXsdFirst
        {
            get
            {
                return thisReadXsdFirst;
            }
        }

        internal bool ReadInstanceFirst
        {
            get
            {
                return thisReadInstanceFirst;
            }
        }

        internal bool ValidityExpected
        {
            get
            {
                return thisValidityExpected;
            }
        }

        internal TestVariation(XmlNode VariationNode)
        {
            XmlNode DescriptionNode = VariationNode.SelectSingleNode("child::description");
            XmlNode DataNode = VariationNode.SelectSingleNode("child::data");
            XmlNode ResultNode = VariationNode.SelectSingleNode("child::result");
            XmlNode XsdNode = DataNode.SelectSingleNode("child::xsd");
            XmlNode InstanceNode = DataNode.SelectSingleNode("child::instance");

            thisId = VariationNode.Attributes["id"].Value;
            thisName = VariationNode.Attributes["name"].Value;
            thisDescription = DescriptionNode.InnerText;
            if (XsdNode != null)
            {
                thisXsd = XsdNode.InnerText;
                string ReadXsdFirstText = XsdNode.Attributes["readMeFirst"].Value;
                if (ReadXsdFirstText == "true")
                    thisReadXsdFirst = true;
                else
                    thisReadXsdFirst = false;
            }
            else
            {
                thisXsd = string.Empty;
                thisReadXsdFirst = false;
            }
            thisInstance = InstanceNode.InnerText;

            string ReadInstanceFirstText = InstanceNode.Attributes["readMeFirst"].Value;
            if (ReadInstanceFirstText == "true")
                thisReadInstanceFirst = true;
            else
                thisReadInstanceFirst = false;

            string ExpectedResultText = ResultNode.Attributes["expected"].Value;
            if (ExpectedResultText == "valid")
                thisValidityExpected = true;
            else
                thisValidityExpected = false;
        }
    }
}

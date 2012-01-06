using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Test.Gepsio
{
    internal class XBRLCONFCR320070305TestVariation
    {
		internal string Id { get; private set; }
        internal string Name { get; private set; }
        internal string Description { get; private set; }
        internal string Xsd { get; private set; }
        internal string Instance { get; private set; }
        internal bool ReadXsdFirst { get; private set; }
        internal bool ReadInstanceFirst { get; private set; }
        internal bool ValidityExpected { get; private set; }

        internal XBRLCONFCR320070305TestVariation(XmlNode VariationNode)
        {
            var descriptionNode = VariationNode.SelectSingleNode("child::description");
            var dataNode = VariationNode.SelectSingleNode("child::data");
            var resultNode = VariationNode.SelectSingleNode("child::result");
            var xsdNode = dataNode.SelectSingleNode("child::xsd");
            var instanceNode = dataNode.SelectSingleNode("child::instance");

            this.Id = VariationNode.Attributes["id"].Value;
            this.Name = VariationNode.Attributes["name"].Value;
            this.Description = descriptionNode.InnerText;
            if (xsdNode != null)
            {
                this.Xsd = xsdNode.InnerText;
                this.ReadXsdFirst = false;
                if (xsdNode.Attributes["readMeFirst"] != null)
                {
                    string ReadXsdFirstText = xsdNode.Attributes["readMeFirst"].Value;
                    if (ReadXsdFirstText == "true")
                        this.ReadXsdFirst = true;
                    else
                        this.ReadXsdFirst = false;
                }
            }
            else
            {
                this.Xsd = string.Empty;
                this.ReadXsdFirst = false;
            }

            if (instanceNode != null)
            {
                this.Instance = instanceNode.InnerText;
                string ReadInstanceFirstText = instanceNode.Attributes["readMeFirst"].Value;
                if (ReadInstanceFirstText == "true")
                    this.ReadInstanceFirst = true;
                else
                    this.ReadInstanceFirst = false;

                string ExpectedResultText = resultNode.Attributes["expected"].Value;
                if (ExpectedResultText == "valid")
                    this.ValidityExpected = true;
                else
                    this.ValidityExpected = false;
            }
            else
            {
                 this.Instance = string.Empty;
            }
        }
    }
}

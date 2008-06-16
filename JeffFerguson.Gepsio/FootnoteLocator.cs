using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class FootnoteLocator
    {
        private XmlNode thisLocNode;
        private FootnoteLink thisFootnoteLink;
        private HyperlinkReference thisHref;
        private string thisLabel;

        public FootnoteLink Link
        {
            get
            {
                return thisFootnoteLink;
            }
        }

        public HyperlinkReference Href
        {
            get
            {
                return thisHref;
            }
        }

        public string Label
        {
            get
            {
                return thisLabel;
            }
        }

        internal FootnoteLocator(FootnoteLink ParentLink, XmlNode LocNode)
        {
            thisLocNode = LocNode;
            thisFootnoteLink = ParentLink;
            foreach (XmlAttribute CurrentAttribute in LocNode.Attributes)
            {
                if (CurrentAttribute.LocalName.Equals("href") == true)
                {
                    string AttributeValue;

                    AttributeValue = CurrentAttribute.Value;
                    thisHref = new HyperlinkReference(AttributeValue);
                }
                else if (CurrentAttribute.LocalName.Equals("label") == true)
                {
                    thisLabel = CurrentAttribute.Value;
                }
            }
        }
    }
}

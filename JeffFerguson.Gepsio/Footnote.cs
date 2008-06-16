using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;

namespace JeffFerguson.Gepsio
{
    public class Footnote
    {
        private XmlNode thisFootnoteNode;
        private FootnoteLink thisFootnoteLink;
        private string thisTitle;
        private string thisLabel;
        private string thisText;
        private CultureInfo thisCulture;

        public FootnoteLink Link
        {
            get
            {
                return thisFootnoteLink;
            }
        }

        public string Title
        {
            get
            {
                return thisTitle;
            }
        }

        public string Label
        {
            get
            {
                return thisLabel;
            }
        }

        public string Text
        {
            get
            {
                return thisText;
            }
        }

        public CultureInfo Culture
        {
            get
            {
                return thisCulture;
            }
        }

        internal Footnote(FootnoteLink ParentLink, XmlNode FootnoteNode)
        {
            thisFootnoteNode = FootnoteNode;
            thisFootnoteLink = ParentLink;
            thisText = thisFootnoteNode.FirstChild.Value;
            thisCulture = null;
            foreach (XmlAttribute CurrentAttribute in thisFootnoteNode.Attributes)
            {
                if (CurrentAttribute.LocalName.Equals("title") == true)
                    thisTitle = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("label") == true)
                    thisLabel = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("lang") == true)
                    thisCulture = new CultureInfo(CurrentAttribute.Value);
            }
            if (thisCulture == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("NoLangForFootnote");
                MessageBuilder.AppendFormat(StringFormat, thisLabel);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }
    }
}

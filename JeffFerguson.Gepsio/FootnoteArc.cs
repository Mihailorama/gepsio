using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class FootnoteArc
    {
        private XmlNode thisFootnoteArcNode;
        private FootnoteLink thisFootnoteLink;
        private string thisTitle;
        private string thisFromId;
        private string thisToId;
        private Fact thisFrom;
        private Footnote thisTo;

        public FootnoteLink Link
        {
            get
            {
                return thisFootnoteLink;
            }
        }

        public Fact From
        {
            get
            {
                return thisFrom;
            }
            internal set
            {
                thisFrom = value;
            }
        }

        public string Title
        {
            get
            {
                return thisTitle;
            }
        }

        public string FromId
        {
            get
            {
                return thisFromId;
            }
        }

        public string ToId
        {
            get
            {
                return thisToId;
            }
        }

        public Footnote To
        {
            get
            {
                return thisTo;
            }
            internal set
            {
                thisTo = value;
            }
        }

        internal FootnoteArc(FootnoteLink ParentLink, XmlNode FootnoteArcNode)
        {
            thisFootnoteArcNode = FootnoteArcNode;
            thisFootnoteLink = ParentLink;
            foreach (XmlAttribute CurrentAttribute in thisFootnoteArcNode.Attributes)
            {
                if(CurrentAttribute.LocalName.Equals("title") == true)
                    thisTitle = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("from") == true)
                    thisFromId = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("to") == true)
                    thisToId = CurrentAttribute.Value;
            }
        }
    }
}

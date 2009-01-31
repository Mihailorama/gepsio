using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class LabelArc
    {
        private string thisFromId;
        private string thisToId;
        private Locator thisFromLocator;

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
        internal LabelArc(XmlNode LabelArcNode)
        {
            thisFromId = XmlUtilities.GetAttributeValue(LabelArcNode, "http://www.w3.org/1999/xlink", "from");
            thisToId = XmlUtilities.GetAttributeValue(LabelArcNode, "http://www.w3.org/1999/xlink", "to");
        }
    }
}

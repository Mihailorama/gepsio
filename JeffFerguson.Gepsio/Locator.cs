using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Locator
    {
        private string thisHref;
        private string thisLabel;
        private string thisTitle;
        private string thisHrefDocumentUri;
        private string thisHrefResourceId;

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string Href
        {
            get
            {
                return thisHref;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string Label
        {
            get
            {
                return thisLabel;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string Title
        {
            get
            {
                return thisTitle;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string HrefDocumentUri
        {
            get
            {
                return thisHrefDocumentUri;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        public string HrefResourceId
        {
            get
            {
                return thisHrefResourceId;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal Locator(XmlNode LocatorNode)
        {
            foreach (XmlAttribute CurrentAttribute in LocatorNode.Attributes)
            {
                if (CurrentAttribute.NamespaceURI.Equals("http://www.w3.org/1999/xlink") == false)
                    continue;
                if (CurrentAttribute.LocalName.Equals("href") == true)
                {
                    thisHref = CurrentAttribute.Value;
                    ParseHref();
                }
                else if (CurrentAttribute.LocalName.Equals("label") == true)
                    thisLabel = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("title") == true)
                    thisTitle = CurrentAttribute.Value;
            }
        }

        //-------------------------------------------------------------------------------
        // Returns true if the supplied href references the same location as the href
        // stored in the Locator, and false otherwise. Note that this method describes
        // a match on the href portion of the locator, not the ID portion.
        //-------------------------------------------------------------------------------
        internal bool HrefEquals(string HrefMatchCandidate)
        {
            if (HrefMatchCandidate.Length == 0)
                return true;
            if (thisHrefDocumentUri.Length < HrefMatchCandidate.Length)
                return HrefMatchCandidate.EndsWith(thisHrefDocumentUri);
            return thisHref.Equals(HrefMatchCandidate);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ParseHref()
        {
            string [] HrefSplit = thisHref.Split(new char[] {'#'});
            thisHrefDocumentUri = HrefSplit[0];
            if (HrefSplit.Length > 1)
                thisHrefResourceId = HrefSplit[1];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class HyperlinkReference
    {
        private string thisHrefValue;
        private string thisUrl;
        private string thisElementId;

        public string Url
        {
            get
            {
                return thisUrl;
            }
        }

        public bool UrlSpecified
        {
            get
            {
                if (thisUrl.Length == 0)
                    return false;
                return true;
            }
        }

        public string ElementId
        {
            get
            {
                return thisElementId;
            }
        }

        public bool ElementIdSpecified
        {
            get
            {
                if (thisElementId.Length == 0)
                    return false;
                return true;
            }
        }

        internal HyperlinkReference(string Href)
        {
            thisHrefValue = Href;
            int PoundSignIndex = thisHrefValue.IndexOf('#');
            if (PoundSignIndex == -1)
            {
                thisElementId = thisHrefValue;
                thisUrl = string.Empty;
                return;
            }
            if (PoundSignIndex == 0)
            {
                thisUrl = string.Empty;
                thisElementId = thisHrefValue.Substring(1);
                return;
            }
            char [] Pound = { '#' };
            string[] Values = thisHrefValue.Split(Pound);
            thisUrl = Values[0];
            thisElementId = Values[1];
        }

        public override string ToString()
        {
            return thisHrefValue;
        }
    }
}

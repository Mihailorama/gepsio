using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    internal class UriPrefixDictionary
    {
        //-------------------------------------------------------------------------------
        // KEY..: Uri
        // VALUE: Prefix
        //-------------------------------------------------------------------------------
        private Dictionary<string, string> thisUriPrefixes;

        internal UriPrefixDictionary()
        {
            thisUriPrefixes = new Dictionary<string, string>();
        }

        internal void Load(XmlNode Node)
        {
            foreach (XmlAttribute CurrentAttribute in Node.Attributes)
                if (CurrentAttribute.Prefix == "xmlns")
                    thisUriPrefixes.Add(CurrentAttribute.Value, CurrentAttribute.LocalName);
        }

        internal string GetPrefixForUri(string Uri)
        {
            string Prefix;

            thisUriPrefixes.TryGetValue(Uri, out Prefix);
            return Prefix;
        }

        internal string GetUriForPrefix(string Prefix)
        {
            string Uri = string.Empty;

            foreach (KeyValuePair<string, string> CurrentPair in thisUriPrefixes)
            {
                if (CurrentPair.Value.Equals(Prefix) == true)
                {
                    Uri = CurrentPair.Key;
                    break;
                }
            }
            return Uri;
        }
    }
}

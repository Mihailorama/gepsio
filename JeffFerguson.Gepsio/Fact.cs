using System;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Fact
    {
        protected XbrlFragment thisParentFragment;
        protected XmlNode thisFactNode;
        private string thisName;

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string Name
        {
            get
            {
                return thisName;
            }
        }

        internal Fact(XbrlFragment ParentFragment, XmlNode FactNode)
        {
            thisParentFragment = ParentFragment;
            thisFactNode = FactNode;
            thisName = thisFactNode.LocalName;
        }

        internal static Fact Create(XbrlFragment ParentFragment, XmlNode FactNode)
        {
            Fact FactToReturn = null;

            if ((IsXbrlNamespace(FactNode.NamespaceURI) == false)
                && (IsW3Namespace(FactNode.NamespaceURI) == false)
                && (FactNode.NodeType != XmlNodeType.Comment))
            {

                // This item could be a fact, or it could be a tuple. Examine the schemas
                // to find out what we're dealing with.

                Element MatchingElement = null;
                foreach (var CurrentSchema in ParentFragment.Schemas)
                {
                    var FoundElement = CurrentSchema.GetElement(FactNode.LocalName);
                    if (FoundElement != null)
                    {
                        MatchingElement = FoundElement;
                    }
                }
                if (MatchingElement != null)
                {
                    switch (MatchingElement.SubstitutionGroup)
                    {
                        case Element.ElementSubstitutionGroup.Item:
                            FactToReturn = new Item(ParentFragment, FactNode);
                            break;
                        case Element.ElementSubstitutionGroup.Tuple:
                            FactToReturn = new Tuple(ParentFragment, FactNode);
                            break;
                        default:
                            // This type is unknown, so leave it alone.
                            break;
                    }
                }
            }
            return FactToReturn;
        }

        /// <summary>
        /// Determines whether or not a namespace URI is hosted by the www.xbrl.org domain.
        /// </summary>
        /// <param name="CandidateNamespace">
        /// A namespace URI.
        /// </param>
        /// <returns>
        /// True if the namespace URI is hosted by the www.xbrl.org domain; false otherwise.
        /// </returns>
        private static bool IsXbrlNamespace(string CandidateNamespace)
        {
            return NamespaceMatchesUri(CandidateNamespace, "www.xbrl.org");
        }

        /// <summary>
        /// Determines whether or not a namespace URI is hosted by the www.w3.org domain.
        /// </summary>
        /// <param name="CandidateNamespace">
        /// A namespace URI.
        /// </param>
        /// <returns>
        /// True if the namespace URI is hosted by the www.w3.org domain; false otherwise.
        /// </returns>
        private static bool IsW3Namespace(string CandidateNamespace)
        {
            return NamespaceMatchesUri(CandidateNamespace, "www.w3.org");
        }

        private static bool NamespaceMatchesUri(string CandidateNamespace, string Uri)
        {
            CandidateNamespace = CandidateNamespace.Trim();
            if (CandidateNamespace.Length == 0)
                return false;
            Uri NamespaceUri = new Uri(CandidateNamespace);
            return NamespaceUri.Host.ToLower().Equals(Uri);
        }
    }
}

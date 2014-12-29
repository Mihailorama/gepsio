using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a definition linkbase document.
    /// </summary>
    public class DefinitionLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="DefinitionLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<DefinitionLink> DefinitionLinks
        {
            get;
            private set;
        }

        DefinitionLinkbaseDocument(XbrlSchema ContainingXbrlSchema, string DocumentPath)
            : base(ContainingXbrlSchema, DocumentPath)
        {
        }
    }
}

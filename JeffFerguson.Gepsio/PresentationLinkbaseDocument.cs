using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a presentation linkbase document.
    /// </summary>
    public class PresentationLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="PresentationLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<PresentationLink> PresentationLinks
        {
            get;
            private set;
        }

        PresentationLinkbaseDocument(XbrlSchema ContainingXbrlSchema, string DocumentPath)
            : base(ContainingXbrlSchema, DocumentPath)
        {
        }
    }
}

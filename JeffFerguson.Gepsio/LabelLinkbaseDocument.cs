using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a label linkbase document.
    /// </summary>
    public class LabelLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="LabelLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<LabelLink> LabelLinks
        {
            get;
            private set;
        }

        LabelLinkbaseDocument(XbrlSchema ContainingXbrlSchema, string DocumentPath)
            : base(ContainingXbrlSchema, DocumentPath)
        {
        }    }
}

using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a calculation linkbase document.
    /// </summary>
    public class CalculationLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="CalculationLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<CalculationLink> CalculationLinks
        {
            get;
            private set;
        }

        CalculationLinkbaseDocument(XbrlSchema ContainingXbrlSchema, string DocumentPath)
            : base(ContainingXbrlSchema, DocumentPath)
        {
        }
    }
}

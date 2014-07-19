using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A collection of XBRL facts.
    /// </summary>
    public class Tuple : Fact
    {
        /// <summary>
        /// A collection of <see cref="Fact"/> objects that are contained by the tuple.
        /// </summary>
        public List<Fact> Facts
        {
            get;
            set;
        }

        internal Tuple(XbrlFragment ParentFragment, INode TupleNode) : base(ParentFragment, TupleNode)
        {
            this.Facts = new List<Fact>();
            foreach (INode CurrentChild in TupleNode.ChildNodes)
            {
                var CurrentFact = Fact.Create(ParentFragment, CurrentChild);
                if (CurrentFact != null)
                    this.Facts.Add(CurrentFact);
            }
        }
    }
}

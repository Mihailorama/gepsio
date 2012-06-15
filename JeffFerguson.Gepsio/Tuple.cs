using System.Collections.Generic;
using System.Xml;

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

        internal Tuple(XbrlFragment ParentFragment, XmlNode TupleNode) : base(ParentFragment, TupleNode)
        {
            this.Facts = new List<Fact>();
            foreach (XmlNode CurrentChild in TupleNode.ChildNodes)
            {
                var CurrentFact = Fact.Create(ParentFragment, CurrentChild);
                if (CurrentFact != null)
                    this.Facts.Add(CurrentFact);
            }
        }
    }
}

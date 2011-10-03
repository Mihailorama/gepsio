using System.Xml;
using System;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    public class Tuple : Fact
    {
        private List<Fact> thisFacts;

        public List<Fact> Facts
        {
            get
            {
                return thisFacts;
            }
            set
            {
                thisFacts = value;
            }
        }

        internal Tuple(XbrlFragment ParentFragment, XmlNode TupleNode) : base(ParentFragment, TupleNode)
        {
            thisFacts = new List<Fact>();
            foreach (XmlNode CurrentChild in TupleNode.ChildNodes)
            {
                var CurrentFact = Fact.Create(ParentFragment, CurrentChild);
                if (CurrentFact != null)
                    thisFacts.Add(CurrentFact);
            }
        }
    }
}

using System.Xml;

namespace JeffFerguson.Gepsio
{
	/// <summary>
	/// An encapsulation of the XBRL element "footnoteArc" as defined in the http://www.xbrl.org/2003/linkbase namespace. 
	/// </summary>
    public class FootnoteArc
    {
        private XmlNode thisFootnoteArcNode;

		/// <summary>
		/// The link definition for the footnote.
		/// </summary>
		public FootnoteLink Link
		{
			get;
			private set;
		}

		/// <summary>
		/// The item referenced by the "from" portion of the footnote arc.
		/// </summary>
		public Item From
		{
			get;
			internal set;
		}

		/// <summary>
		/// The title of the footnote arc.
		/// </summary>
		public string Title
		{
			get;
			private set;
		}

		/// <summary>
		/// The ID of the item referenced by the "from" portion of the footnote arc.
		/// </summary>
		public string FromId
		{
			get;
			private set;
		}

		/// <summary>
		/// The ID of the item referenced by the "to" portion of the footnote arc.
		/// </summary>
		public string ToId
		{
			get;
			private set;
		}

		/// <summary>
		/// The footnote referenced by the "to" portion of the footnote arc.
		/// </summary>
		public Footnote To
		{
			get;
			internal set;
		}

        internal FootnoteArc(FootnoteLink ParentLink, XmlNode FootnoteArcNode)
        {
            thisFootnoteArcNode = FootnoteArcNode;
            this.Link = ParentLink;
            foreach (XmlAttribute CurrentAttribute in thisFootnoteArcNode.Attributes)
            {
                if(CurrentAttribute.LocalName.Equals("title") == true)
                    this.Title = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("from") == true)
                    this.FromId = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("to") == true)
                    this.ToId = CurrentAttribute.Value;
            }
        }
    }
}

using System.Globalization;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
	/// <summary>
	/// A footnote as defined within an XBRL document.
	/// </summary>
    public class Footnote
    {
        private XmlNode thisFootnoteNode;

		/// <summary>
		/// The link associated with this footnote.
		/// </summary>
		public FootnoteLink Link
		{
			get;
			private set;
		}

		/// <summary>
		/// The title for this footnote.
		/// </summary>
		public string Title
		{
			get;
			private set;
		}

		/// <summary>
		/// The label of this footnote.
		/// </summary>
		public string Label
		{
			get;
			private set;
		}

		/// <summary>
		/// The text of this footnote.
		/// </summary>
		public string Text
		{
			get;
			private set;
		}

		/// <summary>
		/// The culture of this footnote.
		/// </summary>
		public CultureInfo Culture
		{
			get;
			private set;
		}

        internal Footnote(FootnoteLink ParentLink, XmlNode FootnoteNode)
        {
            thisFootnoteNode = FootnoteNode;
            this.Link = ParentLink;
            this.Text = thisFootnoteNode.FirstChild.Value;
            this.Culture = null;
            foreach (XmlAttribute CurrentAttribute in thisFootnoteNode.Attributes)
            {
                if (CurrentAttribute.LocalName.Equals("title") == true)
                    this.Title = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("label") == true)
                    this.Label = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("lang") == true)
                    this.Culture = new CultureInfo(CurrentAttribute.Value);
            }
            if (this.Culture == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("NoLangForFootnote");
                MessageBuilder.AppendFormat(StringFormat, this.Label);
                throw new XbrlException(MessageBuilder.ToString());
            }
        }
    }
}

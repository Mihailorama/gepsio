using System.Collections.Generic;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An XML document containing one or more XBRL fragments.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Typically, XBRL documents are XML documents with a single &lt;xbrl&gt; element at the root of the
    /// XML document. However, the XBRL community does provide for XBRL fragments to appear in higher level
    /// documents. For example, Inline XBRL provides a formal specification that governs the production of
    /// web pages in HTML or xHTML that incorporate XBRL tagging instructions around specific facts
    /// commingled inside the HTML markup. This scenario illustrates that a document may not, at its root,
    /// be an XBRL document, but that the root document may contain "fragments" of XBRL. The XbrlDocument
    /// class, therefore, is not simply structured to contain a single XBRL instance, but to contain a
    /// collection of XBRL fragments. The typical XBRL document will contain a single &lt;xbrl&gt; element
    /// at the root of the opened XML document, and, in this case, the XbrlDocument will have a single
    /// XBRL fragment. More complex Inline XBRL scenarios may be represented as an XbrlDocument with one or
    /// more fragments.
    /// </para>
    /// </remarks>
    public class XbrlDocument
    {
        /// <summary>
        /// The URI of the XBRL namespace.
        /// </summary>
        public static string XbrlNamespaceUri = "http://www.xbrl.org/2003/instance";

        private List<XbrlFragment> thisXbrlFragments;
        private string thisFilename;
        private string thisPath;

        /// <summary>
        /// The name of the XML document used to contain the XBRL data.
        /// </summary>
        public string Filename
        {
            get
            {
                return thisFilename;
            }
        }


        /// <summary>
        /// The path to the XML document used to contain the XBRL data.
        /// </summary>
        public string Path
        {
            get
            {
                return thisPath;
            }
        }


        /// <summary>
        /// A collection of <see cref="XbrlFragment"/> objects that contain the document's
        /// XBRL data.
        /// </summary>
        public List<XbrlFragment> XbrlFragments
        {
            get
            {
                return thisXbrlFragments;
            }
        }

        /// <summary>
        /// The constructor for the XbrlDocument class.
        /// </summary>
        public XbrlDocument()
        {
            thisXbrlFragments = new List<XbrlFragment>();
        }

        /// <summary>
        /// Loads an XBRL document containing XBRL data.
        /// </summary>
        /// <param name="Filename">
        /// The filename of the XML document to load.
        /// </param>
        public void Load(string Filename)
        {
            XmlDocument SchemaValidXbrl = new XmlDocument();
            SchemaValidXbrl.Load(Filename);
            thisFilename = Filename;
            thisPath = System.IO.Path.GetDirectoryName(thisFilename);
            XmlNamespaceManager NewNamespaceManager = new XmlNamespaceManager(SchemaValidXbrl.NameTable);
            NewNamespaceManager.AddNamespace("instance", XbrlNamespaceUri);
            XmlNodeList XbrlNodes = SchemaValidXbrl.SelectNodes("//instance:xbrl", NewNamespaceManager);
            foreach (XmlNode XbrlNode in XbrlNodes)
                thisXbrlFragments.Add(new XbrlFragment(this, XbrlNode));
        }
    }
}

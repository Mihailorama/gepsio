using System.Collections.Generic;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An XML document containing one or more XBRL fragments.
    /// </summary>
	/// <remarks>
	/// <para>
	/// An XBRL fragment is a fragment of XBRL data having an xbrl tag as its root. In the generic case,
	/// an XBRL document will have an xbrl tag as the root tag of the XML document, and, in this case,
	/// the entire XBRL document is one large XBRL fragment. However, section 4.1 of the XBRL 2.1 Specification
	/// makes provisions for multiple XBRL fragments to be stored in a single document:
	/// </para>
	/// <para>
	/// "If multiple 'data islands' of XBRL mark-up are included in a larger document, the xbrl element is
	/// the container for each [fragment]."
	/// </para>
	/// <para>
	/// Gepsio supports this notion by defining an XBRL document containing a collection of one or more
	/// XBRL fragments, as in the following code sample:
	/// </para>
	/// <code>
	/// var myDocument = new XbrlDocument();
	/// myDocument.Load("myxbrldoc.xml");
	/// foreach(var currentFragment in myDocument.XbrlFragments)
	/// {
	///     // XBRL data is available from the "currentFragment" variable
	/// }
	/// </code>
	/// <para>
	/// In the vast majority of cases, an XBRL document will be an XML document with the xbrl tag at its
	/// root, and, as a result, the <see cref="XbrlDocument"/> uses to load the XBRL document will have
	/// a single <see cref="XbrlFragment"/> in the document's fragments container. Consider, however, the
	/// possibility of having more than one fragment in a document, in accordance of the text in section
	/// 4.1 of the XBRL 2.1 Specification.
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
        /// Evaluates to true if the document contains no XBRL validation errors. Evaluates to
        /// false if the document contains at least one XBRL validation error.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (thisXbrlFragments == null)
                    return true;
                if (thisXbrlFragments.Count == 0)
                    return true;
                foreach (var currentFragment in thisXbrlFragments)
                {
                    if (currentFragment.IsValid == false)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// A collection of all validation errors found while validating the fragment.
        /// </summary>
        public List<ValidationError> ValidationErrors
        {
            get
            {
                if (thisXbrlFragments == null)
                    return null;
                if (thisXbrlFragments.Count == 0)
                    return null;
                if (thisXbrlFragments.Count == 1)
                    return thisXbrlFragments[0].ValidationErrors;
                var aggregatedValidationErrors = new List<ValidationError>();
                foreach (var currentFragment in thisXbrlFragments)
                {
                    aggregatedValidationErrors.AddRange(currentFragment.ValidationErrors);
                }
                return aggregatedValidationErrors;
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

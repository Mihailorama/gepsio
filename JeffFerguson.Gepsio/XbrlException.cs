using System;

namespace JeffFerguson.Gepsio
{
	/// <summary>
	/// An exception class used to report any XBRL structure or validation errors found in loaded XBRL documents.
	/// </summary>
	/// <remarks>
	/// <para>
	/// XBRL exceptions are most likely to be thrown when a document is loaded. The loading of XBRL documents should
	/// be enclosed in a try/catch block:
	/// <code>
	/// try
	/// {
	///     var myDocument = new XbrlDocument();
	///     myDocument.Load("myxbrldoc.xml");
	/// }
	/// catch(XbrlException e)
	/// {
	///     // load or validation error
	/// }
	/// </code>
	/// </para>
	/// </remarks>
	public class XbrlException : Exception
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Message">
		/// A message to be included as part of the exception.
		/// </param>
		public XbrlException(string Message)
			: base(Message)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Message">
		/// A message to be included as part of the exception.
		/// </param>
		/// <param name="Inner">
		/// An inner exception object to be included as part of the exception.
		/// </param>
		public XbrlException(string Message, Exception Inner)
			: base(Message, Inner)
		{
		}
	}
}

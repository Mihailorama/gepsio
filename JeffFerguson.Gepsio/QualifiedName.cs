using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class QualifiedName : AnySimpleType<string>
    {
        private string thisLocalName;
        private string thisNamespace;
        private string thisNamespaceUri;

        public string LocalName
        {
            get
            {
                return thisLocalName;
            }
        }

        public string Namespace
        {
            get
            {
                return thisNamespace;
            }
        }

        public string NamespaceUri
        {
            get
            {
                return thisNamespaceUri;
            }
        }

        internal QualifiedName(XmlNode QnameNode)
        {
            InitializeLocalNameAndNamespace(QnameNode);
            if (thisNamespace.Length > 0)
                InitializeNamespaceUri(QnameNode);
        }

        private void InitializeNamespaceUri(XmlNode QnameNode)
        {
            if (thisNamespace.Length == 0)
            {
                thisNamespaceUri = string.Empty;
                return;
            }
            string AttributeName = "xmlns:" + thisNamespace;
            thisNamespaceUri = XmlUtilities.GetAttributeValue(QnameNode, AttributeName);
        }

        private void InitializeLocalNameAndNamespace(XmlNode QnameNode)
        {
            string[] InnerTextSplit = QnameNode.InnerText.Split(':');
            if (InnerTextSplit.Length == 1)
            {
                thisNamespace = string.Empty;
                thisLocalName = InnerTextSplit[0];
            }
            else
            {
                thisNamespace = InnerTextSplit[0];
                thisLocalName = InnerTextSplit[1];
            }
        }

        protected override string ConvertStringValue()
        {
            return ValueAsString;
        }
    }
}

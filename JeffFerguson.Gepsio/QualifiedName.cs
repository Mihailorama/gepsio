using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class QualifiedName : AnySimpleType
    {
        private string thisLocalName;
        private string thisNamespace;
        private string thisNamespaceUri;
        private string thisToStringValue;

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
            thisToStringValue = QnameNode.InnerText;
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

        public override bool Equals(object obj)
        {
            if ((obj is QualifiedName) == false)
                return false;
            QualifiedName OtherObj = obj as QualifiedName;
            if(thisLocalName.Equals(OtherObj.thisLocalName) == false)
                return false;
            if(thisNamespace.Equals(OtherObj.thisNamespace) == false)
                return false;
            if(thisNamespaceUri.Equals(OtherObj.thisNamespaceUri) == false)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            return thisLocalName.GetHashCode();
        }

        public override string ToString()
        {
            return thisToStringValue;
        }
    }
}

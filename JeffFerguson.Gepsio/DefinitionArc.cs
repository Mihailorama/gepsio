using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class DefinitionArc
    {
        public enum RoleEnum
        {
            Unknown,
            EssenceAlias,
            GeneralSpecial,
            SimilarTuples,
            RequiresElement
        }

        private RoleEnum thisRole;
        private string thisTitle;
        private string thisFromId;
        private string thisToId;
        private Locator thisFromLocator;
        private Locator thisToLocator;

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public RoleEnum Role
        {
            get
            {
                return thisRole;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string FromId
        {
            get
            {
                return thisFromId;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string ToId
        {
            get
            {
                return thisToId;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public Locator FromLocator
        {
            get
            {
                return thisFromLocator;
            }
            internal set
            {
                thisFromLocator = value;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public Locator ToLocator
        {
            get
            {
                return thisToLocator;
            }
            internal set
            {
                thisToLocator = value;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal DefinitionArc(XmlNode DefinitionArcNode)
        {
            foreach (XmlAttribute CurrentAttribute in DefinitionArcNode.Attributes)
            {
                if (CurrentAttribute.NamespaceURI.Equals("http://www.w3.org/1999/xlink") == false)
                    continue;
                if (CurrentAttribute.LocalName.Equals("arcrole") == true)
                    SetRole(CurrentAttribute.Value);
                else if (CurrentAttribute.LocalName.Equals("title") == true)
                    thisTitle = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("from") == true)
                    thisFromId = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("to") == true)
                    thisToId = CurrentAttribute.Value;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void SetRole(string ArcRoleValue)
        {
            thisRole = RoleEnum.Unknown;
            if (ArcRoleValue.Equals("http://www.xbrl.org/2003/arcrole/essence-alias") == true)
                thisRole = RoleEnum.EssenceAlias;
            else if (ArcRoleValue.Equals("http://www.xbrl.org/2003/arcrole/general-special") == true)
                thisRole = RoleEnum.GeneralSpecial;
            else if (ArcRoleValue.Equals("http://www.xbrl.org/2003/arcrole/similar-tuples") == true)
                thisRole = RoleEnum.SimilarTuples;
            else if (ArcRoleValue.Equals("http://www.xbrl.org/2003/arcrole/requires-element") == true)
                thisRole = RoleEnum.RequiresElement;
        }
    }
}

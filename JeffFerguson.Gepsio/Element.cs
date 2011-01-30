using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio
{
    public class Element
    {
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public enum ElementSubstitutionGroup
        {
            Unknown,
            Item,
            Tuple,
            DimensionItem,
            HypercubeItem
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public enum ElementPeriodType
        {
            Unknown,
            Instant,
            Duration
        }

        private XmlSchemaElement thisSchemaElement;
        private XmlNode thisElementNode;
        private string thisName;
        private string thisId;
        private XbrlSchema thisSchema;
        private ElementSubstitutionGroup thisSubstitutionGroup;
        private ElementPeriodType thisPeriodType;
        private XmlQualifiedName thisTypeName;

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string Name
        {
            get
            {
                return thisName;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public string Id
        {
            get
            {
                return thisId;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public ElementSubstitutionGroup SubstitutionGroup
        {
            get
            {
                return thisSubstitutionGroup;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public ElementPeriodType PeriodType
        {
            get
            {
                return thisPeriodType;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public XbrlSchema Schema
        {
            get
            {
                return thisSchema;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public XmlQualifiedName TypeName
        {
            get
            {
                return thisTypeName;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal Element(XbrlSchema Schema, XmlNode ElementNode)
        {
            throw new NotSupportedException("no more hardcoded parsing of schema elements!");
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal Element(XbrlSchema Schema, XmlSchemaElement SchemaElement)
        {
            thisSchema = Schema;
            thisSchemaElement = SchemaElement;
            thisId = SchemaElement.Id;
            thisName = SchemaElement.Name;
            thisTypeName = SchemaElement.SchemaTypeName;
            SetSubstitutionGroup(SchemaElement.SubstitutionGroup);
            SetPeriodType();
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            if ((obj is Element) == false)
                return false;
            Element OtherElement = obj as Element;
            return OtherElement.Id.Equals(this.Id);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void SetPeriodType()
        {
            string PeriodTypePrefix = thisSchema.GetPrefixForUri("http://www.xbrl.org/2003/instance");
            string AttributeName;
            if (PeriodTypePrefix == null)
                AttributeName = "periodType";
            else
                AttributeName = PeriodTypePrefix + ":periodType";
            thisPeriodType = ElementPeriodType.Unknown;
            if (thisSchemaElement.UnhandledAttributes != null)
            {
                foreach (XmlAttribute CurrentAttribute in thisSchemaElement.UnhandledAttributes)
                {
                    if (CurrentAttribute.Name.Equals(AttributeName) == true)
                    {
                        SetPeriodType(CurrentAttribute.Value);
                        return;
                    }
                }
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void SetPeriodType(string PeriodType)
        {
            thisPeriodType = ElementPeriodType.Unknown;
            if (PeriodType == "instant")
                thisPeriodType = ElementPeriodType.Instant;
            else if (PeriodType == "duration")
                thisPeriodType = ElementPeriodType.Duration;
            else
            {

                // We can't identify the type, so throw an exception.

                string MessageFormat = AssemblyResources.GetName("InvalidElementPeriodType");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, thisSchema.Path, PeriodType, thisName);
                throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void SetSubstitutionGroup(XmlQualifiedName SubstitutionGroupValue)
        {
            thisSubstitutionGroup = ElementSubstitutionGroup.Unknown;
            if ((SubstitutionGroupValue.Name.Length == 0) && (SubstitutionGroupValue.Namespace.Length == 0))
                return;
            if((SubstitutionGroupValue.Namespace.Equals("http://www.xbrl.org/2003/instance") == true) && (SubstitutionGroupValue.Name.Equals("item") == true))
                thisSubstitutionGroup = ElementSubstitutionGroup.Item;
            else if ((SubstitutionGroupValue.Namespace.Equals("http://www.xbrl.org/2003/instance") == true) && (SubstitutionGroupValue.Name.Equals("tuple") == true))
                thisSubstitutionGroup = ElementSubstitutionGroup.Tuple;
            else if ((SubstitutionGroupValue.Namespace.Equals("http://xbrl.org/2005/xbrldt") == true) && (SubstitutionGroupValue.Name.Equals("dimensionItem") == true))
                thisSubstitutionGroup = ElementSubstitutionGroup.DimensionItem;
            else if ((SubstitutionGroupValue.Namespace.Equals("http://xbrl.org/2005/xbrldt") == true) && (SubstitutionGroupValue.Name.Equals("hypercubeItem") == true))
                thisSubstitutionGroup = ElementSubstitutionGroup.HypercubeItem;
            else
            {

                //// We can't identify the type, so throw an exception.

                //string MessageFormat = AssemblyResources.GetName("InvalidElementSubstitutionGroup");
                //StringBuilder MessageFormatBuilder = new StringBuilder();
                //MessageFormatBuilder.AppendFormat(MessageFormat, thisSchema.Path, SubstitutionGroupValue, thisName);
                //throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }
    }
}

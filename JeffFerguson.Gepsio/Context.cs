using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
    public class Context
    {
        private XmlNode thisContextNode;
        private string thisId;
        private bool thisForeverPeriod;
        private bool thisInstantPeriod;
        private XmlNode thisInstantPeriodNode;
        private bool thisDurationPeriod;
        private XmlNode thisStartDateDurationNode;
        private XmlNode thisEndDateDurationNode;
        private string thisIdentifier;
        private string thisIdentifierScheme;
        private XmlNode thisSegmentNode;
        private XmlNode thisScenarioNode;
        private XbrlFragment thisFragment;
        private System.DateTime thisStartDate;
        private System.DateTime thisEndDate;
        private System.DateTime thisInstantDate;

        public string Id
        {
            get
            {
                return thisId;
            }
        }

        public bool InstantPeriod
        {
            get
            {
                return thisInstantPeriod;
            }
        }

        public bool DurationPeriod
        {
            get
            {
                if ((thisForeverPeriod == true) || (thisDurationPeriod == true))
                    return true;
                return false;
            }
        }

        public bool ForeverPeriod
        {
            get
            {
                return thisForeverPeriod;
            }
        }

        public string Identifier
        {
            get
            {
                return thisIdentifier;
            }
        }

        public string IdentifierScheme
        {
            get
            {
                return thisIdentifierScheme;
            }
        }

        public XmlNode Segment
        {
            get
            {
                return thisSegmentNode;
            }
        }

        public XmlNode Scenario
        {
            get
            {
                return thisScenarioNode;
            }
        }

        public XbrlFragment Fragment
        {
            get
            {
                return thisFragment;
            }
        }

        public System.DateTime PeriodStartDate
        {
            get
            {
                return thisStartDate;
            }
        }

        public System.DateTime PeriodEndDate
        {
            get
            {
                return thisEndDate;
            }
        }

        public System.DateTime InstantDate
        {
            get
            {
                return thisInstantDate;
            }
        }

        internal Context(XbrlFragment Fragment, XmlNode ContextNode)
        {
            thisFragment = Fragment;
            thisContextNode = ContextNode;
            thisId = thisContextNode.Attributes["id"].Value;
            thisStartDate = System.DateTime.MinValue;
            thisEndDate = System.DateTime.MinValue;
            foreach (XmlNode CurrentChild in thisContextNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("period") == true)
                {
                    ProcessPeriod(CurrentChild);
                    ValidatePeriod();
                }
                else if (CurrentChild.LocalName.Equals("entity") == true)
                    ProcessEntity(CurrentChild);
                else if (CurrentChild.LocalName.Equals("scenario") == true)
                    ProcessScenario(CurrentChild);
            }
        }

        private void ProcessEntity(XmlNode EntityNode)
        {
            thisIdentifier = string.Empty;
            thisIdentifierScheme = string.Empty;
            thisSegmentNode = null;
            thisScenarioNode = null;
            foreach (XmlNode CurrentChild in EntityNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("identifier") == true)
                    ProcessIdentifier(CurrentChild);
                else if (CurrentChild.LocalName.Equals("segment") == true)
                    ProcessSegment(CurrentChild);

            }
        }

        private void ProcessScenario(XmlNode ScenarioNode)
        {
            thisScenarioNode = ScenarioNode;
            ValidateScenario();
        }

        private void ValidateScenario()
        {
            foreach (XmlNode CurrentChild in thisScenarioNode.ChildNodes)
                ValidateScenarioNode(CurrentChild);
        }

        private void ValidateScenarioNode(XmlNode ScenarioNode)
        {
            if (ScenarioNode.NamespaceURI.Equals("http://www.xbrl.org/2003/instance") == true)
            {
                string MessageFormat = AssemblyResources.GetName("ScenarioNodeUsingXBRLNamespace");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, thisId, ScenarioNode.Name);
                throw new XbrlException(MessageBuilder.ToString());
            }
            if (ScenarioNode.Prefix.Length > 0)
            {
                XbrlSchema NodeSchema = thisFragment.GetXbrlSchemaForPrefix(ScenarioNode.Prefix);
                if (NodeSchema != null)
                {
                    Element NodeElement = NodeSchema.GetElement(ScenarioNode.LocalName);
                    if (NodeElement != null)
                    {
                        if (NodeElement.SubstitutionGroup != Element.ElementSubstitutionGroup.Unknown)
                        {
                            string MessageFormat = AssemblyResources.GetName("ScenarioNodeUsingSubGroupInXBRLNamespace");
                            StringBuilder MessageBuilder = new StringBuilder();
                            MessageBuilder.AppendFormat(MessageFormat, thisId, ScenarioNode.Name, NodeSchema.Path);
                            throw new XbrlException(MessageBuilder.ToString());
                        }
                    }
                }
            }
            foreach (XmlNode CurrentChild in ScenarioNode.ChildNodes)
                ValidateScenarioNode(CurrentChild);
        }

        private void ProcessSegment(XmlNode SegmentNode)
        {
            thisSegmentNode = SegmentNode;
            ValidateSegment();
        }

        private void ValidateSegment()
        {
            foreach (XmlNode CurrentChild in thisSegmentNode.ChildNodes)
                ValidateSegmentNode(CurrentChild);
        }

        private void ValidateSegmentNode(XmlNode SegmentNode)
        {
            if (SegmentNode.NamespaceURI.Equals("http://www.xbrl.org/2003/instance") == true)
            {
                string MessageFormat = AssemblyResources.GetName("SegmentNodeUsingXBRLNamespace");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, thisId, SegmentNode.Name);
                throw new XbrlException(MessageBuilder.ToString());
            }
            if (SegmentNode.Prefix.Length > 0)
            {
                XbrlSchema NodeSchema = thisFragment.GetXbrlSchemaForPrefix(SegmentNode.Prefix);
                if (NodeSchema != null)
                {
                    Element NodeElement = NodeSchema.GetElement(SegmentNode.LocalName);
                    if (NodeElement != null)
                    {
                        if (NodeElement.SubstitutionGroup != Element.ElementSubstitutionGroup.Unknown)
                        {
                            string MessageFormat = AssemblyResources.GetName("SegmentNodeUsingSubGroupInXBRLNamespace");
                            StringBuilder MessageBuilder = new StringBuilder();
                            MessageBuilder.AppendFormat(MessageFormat, thisId, SegmentNode.Name, NodeSchema.Path);
                            throw new XbrlException(MessageBuilder.ToString());
                        }
                    }
                }
            }
            foreach (XmlNode CurrentChild in SegmentNode.ChildNodes)
                ValidateSegmentNode(CurrentChild);
        }

        private void ProcessIdentifier(XmlNode IdentifierNode)
        {
            thisIdentifier = IdentifierNode.InnerText;
            if (IdentifierNode.Attributes["scheme"] != null)
                thisIdentifierScheme = IdentifierNode.Attributes["scheme"].Value;
        }

        private void ProcessPeriod(XmlNode PeriodNode)
        {
            thisInstantPeriod = false;
            thisInstantPeriodNode = null;
            thisForeverPeriod = false;
            thisDurationPeriod = false;
            thisStartDateDurationNode = null;
            thisEndDateDurationNode = null;
            foreach (XmlNode CurrentChild in PeriodNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("instant") == true)
                {
                    thisInstantPeriod = true;
                    thisInstantPeriodNode = CurrentChild;
                    System.DateTime.TryParse(thisInstantPeriodNode.InnerText, out thisInstantDate);
                }
                else if (CurrentChild.LocalName.Equals("forever") == true)
                    thisForeverPeriod = true;
                else if (CurrentChild.LocalName.Equals("startDate") == true)
                {
                    thisDurationPeriod = true;
                    thisStartDateDurationNode = CurrentChild;
                    System.DateTime.TryParse(thisStartDateDurationNode.InnerText, out thisStartDate);
                }
                else if (CurrentChild.LocalName.Equals("endDate") == true)
                {
                    thisEndDateDurationNode = CurrentChild;
                    System.DateTime.TryParse(thisEndDateDurationNode.InnerText, out thisEndDate);
                }
            }
        }

        private void ValidatePeriod()
        {
            if ((thisStartDate != System.DateTime.MinValue) && (thisEndDate != System.DateTime.MinValue))
            {
                if (thisEndDate < thisStartDate)
                {
                    string MessageFormat = AssemblyResources.GetName("PeriodEndDateLessThanPeriodStartDate");
                    StringBuilder MessageBuilder = new StringBuilder();
                    MessageBuilder.AppendFormat(MessageFormat, thisId);
                    throw new XbrlException(MessageBuilder.ToString());
                }
            }
        }

        //------------------------------------------------------------------------------------
        // Returns true if this context is Structure Equal (s-equal) to a supplied context,
        // and false otherwise. See section 4.10 of the XBRL 2.1 spec for more information.
        //------------------------------------------------------------------------------------
        internal bool StructureEquals(Context OtherContext)
        {
            if (PeriodStructureEquals(OtherContext) == false)
                return false;
            if (EntityStructureEquals(OtherContext) == false)
                return false;
            if (ScenarioStructureEquals(OtherContext) == false)
                return false;
            return true;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private bool ScenarioStructureEquals(Context OtherContext)
        {
            if ((this.Scenario == null) && (OtherContext.Scenario == null))
                return true;
            if ((this.Scenario == null) && (OtherContext.Scenario != null))
                return true;
            if ((this.Scenario != null) && (OtherContext.Scenario == null))
                return true;
            return this.Scenario.StructureEquals(OtherContext.Scenario);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private bool EntityStructureEquals(Context OtherContext)
        {
            if (this.Identifier.Equals(OtherContext.Identifier) == false)
                return false;
            if (SegmentStructureEquals(OtherContext) == false)
                return false;
            return true;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private bool SegmentStructureEquals(Context OtherContext)
        {
            //--------------------------------------------------------------------------------
            // If neither context has a <segment> node, then the segments are considered
            // equal.
            //--------------------------------------------------------------------------------
            if ((this.Segment == null) && (OtherContext.Segment == null))
                return true;
            //--------------------------------------------------------------------------------
            // If this context does not have a <segment> node, but the other one does, then
            // the segments are equal only if the other <segment> is empty.
            //--------------------------------------------------------------------------------
            if ((this.Segment == null) && (OtherContext.Segment != null))
            {
                if (OtherContext.Segment.ChildNodes.Count > 0)
                    return false;
                return true;
            }
            //--------------------------------------------------------------------------------
            // If the other context does not have a <segment> node, but this one does, then
            // the segments are equal only if this <segment> is empty.
            //--------------------------------------------------------------------------------
            if ((this.Segment != null) && (OtherContext.Segment == null))
            {
                if (this.Segment.ChildNodes.Count > 0)
                    return false;
                return true;
            }
            //--------------------------------------------------------------------------------
            // At this point, both segments exist. Check to see if they have the same
            // structure.
            //--------------------------------------------------------------------------------
            return this.Segment.StructureEquals(OtherContext.Segment);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private bool PeriodStructureEquals(Context OtherContext)
        {
            if (this.ForeverPeriod != OtherContext.ForeverPeriod)
                return false;
            if (this.InstantPeriod != OtherContext.InstantPeriod)
                return false;
            if (this.DurationPeriod != OtherContext.DurationPeriod)
                return false;
            if (DurationPeriod == true)
            {
                if (this.thisStartDate != OtherContext.thisStartDate)
                    return false;
                if (this.thisEndDate != OtherContext.thisEndDate)
                    return false;
            }
            if (InstantPeriod == true)
            {
                if (this.thisInstantDate != OtherContext.thisInstantDate)
                    return false;
            }
            return true;
        }
    }
}

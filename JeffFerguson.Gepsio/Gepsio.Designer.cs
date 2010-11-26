﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JeffFerguson.Gepsio {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Gepsio {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Gepsio() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("JeffFerguson.Gepsio.Gepsio", typeof(Gepsio).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find a &lt;context&gt; tag for contextRef &apos;{0}&apos;..
        /// </summary>
        internal static string CannotFindContextForContextRef {
            get {
                return ResourceManager.GetString("CannotFindContextForContextRef", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find a fact for element {0}..
        /// </summary>
        internal static string CannotFindFactForElement {
            get {
                return ResourceManager.GetString("CannotFindFactForElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Footnote arc {0} references fact {1}, but no fact with that name can be found..
        /// </summary>
        internal static string CannotFindFactForFootnoteArc {
            get {
                return ResourceManager.GetString("CannotFindFactForFootnoteArc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Footnote arc {0} references footnote {1}, but no footnote with that name can be found..
        /// </summary>
        internal static string CannotFindFootnoteForFootnoteArc {
            get {
                return ResourceManager.GetString("CannotFindFootnoteForFootnoteArc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Footnote arc {0} references a footnote locator with a label of {1}, but the locator cannot be found..
        /// </summary>
        internal static string CannotFindFootnoteLocator {
            get {
                return ResourceManager.GetString("CannotFindFootnoteLocator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find a &lt;unit&gt; tag for unitRef &apos;{0}&apos;..
        /// </summary>
        internal static string CannotFindUnitForUnitRef {
            get {
                return ResourceManager.GetString("CannotFindUnitForUnitRef", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Schema {0} defines fact element {1} with a duration-based period, but context {2}, used by the fact, does not implement a duration-based period..
        /// </summary>
        internal static string ElementSchemaDefinesDurationButUsedWithNonDurationContext {
            get {
                return ResourceManager.GetString("ElementSchemaDefinesDurationButUsedWithNonDurationContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Schema {0} defines fact element {1} with an instant-based period, but context {2}, used by the fact, does not implement an instant-based period..
        /// </summary>
        internal static string ElementSchemaDefinesInstantButUsedWithNonInstantContext {
            get {
                return ResourceManager.GetString("ElementSchemaDefinesInstantButUsedWithNonInstantContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error loading XBRL document..
        /// </summary>
        internal static string ErrorLoadingXbrlDocument {
            get {
                return ResourceManager.GetString("ErrorLoadingXbrlDocument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Facts named {0} are defined as being in an essence alias relationship with facts named {1}. However, the fact with ID {2} is not context equal with the fact with ID {3}. These two facts are therefore not in a valid essence alias relationship..
        /// </summary>
        internal static string EssenceAliasFactsNotContextEquals {
            get {
                return ResourceManager.GetString("EssenceAliasFactsNotContextEquals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Facts named {0} are defined as being in an essence alias relationship with facts named {1}. However, the fact with ID {2} is not parent equal with the fact with ID {3}. These two facts are therefore not in a valid essence alias relationship..
        /// </summary>
        internal static string EssenceAliasFactsNotParentEquals {
            get {
                return ResourceManager.GetString("EssenceAliasFactsNotParentEquals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Facts named {0} are defined as being in an essence alias relationship with facts named {1}. However, the fact with ID {2} is not unit equal with the fact with ID {3}. These two facts are therefore not in a valid essence alias relationship..
        /// </summary>
        internal static string EssenceAliasFactsNotUnitEquals {
            get {
                return ResourceManager.GetString("EssenceAliasFactsNotUnitEquals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Facet creation does not support creation of facets defined by type {0}..
        /// </summary>
        internal static string FacetDefinitionNotSupportedForFacetCreation {
            get {
                return ResourceManager.GetString("FacetDefinitionNotSupportedForFacetCreation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Footnote references fact {0} in external document {1}. Footnotes cannot reference facts in external documents..
        /// </summary>
        internal static string FootnoteReferencesFactInExternalDoc {
            get {
                return ResourceManager.GetString("FootnoteReferencesFactInExternalDoc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Schema {0} contains invalid item type {1} on element {2}..
        /// </summary>
        internal static string InvalidElementItemType {
            get {
                return ResourceManager.GetString("InvalidElementItemType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Schema {0} contains invalid period type value {1} on element {2}..
        /// </summary>
        internal static string InvalidElementPeriodType {
            get {
                return ResourceManager.GetString("InvalidElementPeriodType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Schema {0} contains invalid substitution group value {1} on element {2}..
        /// </summary>
        internal static string InvalidElementSubstitutionGroup {
            get {
                return ResourceManager.GetString("InvalidElementSubstitutionGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fact {0} is defined as a numeric item with a nil value. All numeric-based facts with a nil value must not specify either a precision attribute or a decimals atribute. The fact with ID {1} specifies one or both of these attributes..
        /// </summary>
        internal static string NilNumericFactWithSpecifiedPrecisionOrDecimals {
            get {
                return ResourceManager.GetString("NilNumericFactWithSpecifiedPrecisionOrDecimals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Footnote references a location of {0}, but none of the facts uses that ID..
        /// </summary>
        internal static string NoFactForFootnoteReference {
            get {
                return ResourceManager.GetString("NoFactForFootnoteReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Footnote {0} does not include any language identifier information. Ensure that the footnote includes an xml:lang attribute..
        /// </summary>
        internal static string NoLangForFootnote {
            get {
                return ResourceManager.GetString("NoLangForFootnote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Facts named {0} are defined as being in a requires-element relationship with facts named {1}. However, there are less instances of the {1} fact than of the {0} fact. A requires-element relationship mandates that there be one {1} fact instance for every {0} fact instance..
        /// </summary>
        internal static string NotEnoughToFactsInRequiresElementRelationship {
            get {
                return ResourceManager.GetString("NotEnoughToFactsInRequiresElementRelationship", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fact {0} is defined as a numeric item. All numeric-based facts must specify either a precision attribute or a decimals atribute. The fact with ID {1} does not specify either a precision or a decimals attribute..
        /// </summary>
        internal static string NumericFactWithoutSpecifiedPrecisionOrDecimals {
            get {
                return ResourceManager.GetString("NumericFactWithoutSpecifiedPrecisionOrDecimals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fact {0} is defined as a numeric item. All numeric-based facts must specify either a precision attribute or a decimals atribute. The fact with ID {1} specifies both a precision and a decimals attribute..
        /// </summary>
        internal static string NumericFactWithSpecifiedPrecisionAndDecimals {
            get {
                return ResourceManager.GetString("NumericFactWithSpecifiedPrecisionAndDecimals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Context ID {0} contains period information that specifies a start date that is later than the end date..
        /// </summary>
        internal static string PeriodEndDateLessThanPeriodStartDate {
            get {
                return ResourceManager.GetString("PeriodEndDateLessThanPeriodStartDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fact {0} is defined as a pure item type. The fact also references a unit named {1}. The unit defines a measure referencing a local name of {2}. Local names for units of type pureItemType must be &quot;pure&quot;..
        /// </summary>
        internal static string PureItemTypeUnitLocalNameNotPure {
            get {
                return ResourceManager.GetString("PureItemTypeUnitLocalNameNotPure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Context ID {0} contains a node in its scenario structure named &lt;{1}&gt;. This node is defined in the schema at {2} with a substitution group setting that references the XBRL namespace. XBRL namespace substitution group references are not allowed in context scenarios..
        /// </summary>
        internal static string ScenarioNodeUsingSubGroupInXBRLNamespace {
            get {
                return ResourceManager.GetString("ScenarioNodeUsingSubGroupInXBRLNamespace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Context ID {0} contains a node in its scenario structure named &lt;{1}&gt;. This namespace for this node is the XBRL namespace. XBRL namespace node names are not allowed in context segments..
        /// </summary>
        internal static string ScenarioNodeUsingXBRLNamespace {
            get {
                return ResourceManager.GetString("ScenarioNodeUsingXBRLNamespace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file {0} at is referenced as a XBRL taxonomy schema. However, this file does not contain a root &lt;schema&gt; node and is not a valid XBRL taxonomy schema..
        /// </summary>
        internal static string SchemaFileCandidateDoesNotContainSchemaRootNode {
            get {
                return ResourceManager.GetString("SchemaFileCandidateDoesNotContainSchemaRootNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Context ID {0} contains a node in its segment structure named &lt;{1}&gt;. This node is defined in the schema at {2} with a substitution group setting that references the XBRL namespace. XBRL namespace substitution group references are not allowed in context segments..
        /// </summary>
        internal static string SegmentNodeUsingSubGroupInXBRLNamespace {
            get {
                return ResourceManager.GetString("SegmentNodeUsingSubGroupInXBRLNamespace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Context ID {0} contains a node in its segment structure named &lt;{1}&gt;. This namespace for this node is the XBRL namespace. XBRL namespace node names are not allowed in context segments..
        /// </summary>
        internal static string SegmentNodeUsingXBRLNamespace {
            get {
                return ResourceManager.GetString("SegmentNodeUsingXBRLNamespace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fact {0} is defined as a shares item type. The fact also references a unit named {1}. The unit defines a measure referencing a local name of {2}. Local names for units of type sharesItemType must be &quot;shares&quot;..
        /// </summary>
        internal static string SharesItemTypeUnitLocalNameNotShares {
            get {
                return ResourceManager.GetString("SharesItemTypeUnitLocalNameNotShares", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fact {0} is based on an element that is named as the summation concept in a calculation link. The fact&apos;s value, after precision or decimals truncation, is {1}; however, the sum of the values of the contributing concepts, after precision or decimals truncation, is {2}. These values do not match; therefore, the rule specified by the fact&apos;s calculation link has been broken..
        /// </summary>
        internal static string SummationConceptError {
            get {
                return ResourceManager.GetString("SummationConceptError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unit {0} is defined as a ratio. The ratio makes uses of a measure called {1} in both the numerator and the denominator. Ratios in units must not use the same measure in both the numerator and the denominator..
        /// </summary>
        internal static string UnitRatioUsesSameMeasureInNumeratorAndDenominator {
            get {
                return ResourceManager.GetString("UnitRatioUsesSameMeasureInNumeratorAndDenominator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A facet named {0} was found in a schema and used for XML datatype {1}. This facet is not supported for this XML datatype..
        /// </summary>
        internal static string UnsupportedFacet {
            get {
                return ResourceManager.GetString("UnsupportedFacet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Facet property {0} is not valid for facet {1}. See http://www.w3.org/TR/xmlschema-2/ for more information..
        /// </summary>
        internal static string UnsupportedFacetProperty {
            get {
                return ResourceManager.GetString("UnsupportedFacetProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fact {0} is defined as a monetary item type. The fact also references a unit named {1}. The unit defines a code of {2}, which is to be interpreted as an ISO 4217 currency code. This code is not found in the list of supported ISO 4217 currency codes and is invalid..
        /// </summary>
        internal static string UnsupportedISO4217CodeForUnitMeasure {
            get {
                return ResourceManager.GetString("UnsupportedISO4217CodeForUnitMeasure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Simple type {0} is unsupported as a restriction base type..
        /// </summary>
        internal static string UnsupportedRestrictionBaseSimpleType {
            get {
                return ResourceManager.GetString("UnsupportedRestrictionBaseSimpleType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fact {0} is defined as a monetary item type. The fact also references a unit named {1}. The unit defines a measure referencing a namespace of {2}. This differs from the standard monetary unit namespace of http://www.xbrl.org/2003/iso4217. Monetary-based unit measures must reference the http://www.xbrl.org/2003/iso4217 namespace..
        /// </summary>
        internal static string WrongMeasureNamespaceForMonetaryFact {
            get {
                return ResourceManager.GetString("WrongMeasureNamespaceForMonetaryFact", resourceCulture);
            }
        }
    }
}

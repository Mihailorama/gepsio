using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JeffFerguson.Gepsio
{
	/// <summary>
	/// An encapsulation of the XML schema type "anyType" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
	/// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
	/// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
	/// Gepsio's XML schema type system obsolete.
	/// </para>
	/// </remarks>
    public abstract class AnyType
    {

		/// <summary>
		/// The value of the type, represented as a <see cref="string"/>.
		/// </summary>
		public virtual string ValueAsString
		{
			get;
			set;
		}

		/// <summary>
		/// Describes whether or not this type is a numeric type. Returns true if this type is a numeric type. Returns false if this
		/// type is not a numeric type.
		/// </summary>
        public abstract bool NumericType
        {
            get;
        }

        internal AnyType()
        {
        }

        internal virtual void ValidateFact(Item FactToValidate)
        {
        }

		/// <summary>
		/// Creates a XML schema type for use by the internally-implemented XML schema type system.
		/// </summary>
		/// <param name="TypeName">
		/// The name of the type to be created. Specific types are created according to the following table:
		/// <list type="table">
		/// <listeader>
		/// <term>
		/// Type Name
		/// </term>
		/// <description>
		/// Specific Class of Returned Object
		/// </description>
		/// </listeader>
		/// <item>
		/// <term>
		/// token
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.Token"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// string
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.String"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:decimalItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.DecimalItemType"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:monetaryItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.MonetaryItemType"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:pureItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.PureItemType"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:sharesItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.SharesItemType"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:tokenItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.TokenItemType"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:stringItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.StringItemType"/>
		/// </description>
		/// </item>
		/// </list>
		/// <para>
		/// If a type name not shown above is supplied, then null will be returned.
		/// </para>
		/// </param>
		/// <returns>
		/// A type object representing the type referenced by the parameter, or null if the type name is
		/// not supported.
		/// </returns>
        public static AnyType CreateType(string TypeName)
        {
            return AnyType.CreateType(TypeName, null);
        }

        internal abstract decimal GetValueAfterApplyingPrecisionTruncation(int PrecisionValue);

        internal abstract decimal GetValueAfterApplyingDecimalsTruncation(int DecimalsValue);

		/// <summary>
		/// Creates a XML schema type for use by the internally-implemented XML schema type system.
		/// </summary>
		/// <param name="TypeName">
		/// The name of the type to be created. Specific types are created according to the following table:
		/// <list type="table">
		/// <listeader>
		/// <term>
		/// Type Name
		/// </term>
		/// <description>
		/// Specific Class of Returned Object
		/// </description>
		/// </listeader>
		/// <item>
		/// <term>
		/// token
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.Token"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// string
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.String"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:decimalItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.DecimalItemType"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:monetaryItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.MonetaryItemType"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:pureItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.PureItemType"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:sharesItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.SharesItemType"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:tokenItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.TokenItemType"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// xbrli:stringItemType
		/// </term>
		/// <description>
		/// <see cref="JeffFerguson.Gepsio.StringItemType"/>
		/// </description>
		/// </item>
		/// </list>
		/// <para>
		/// If a type name not shown above is supplied, then null will be returned.
		/// </para>
		/// </param>
		/// <param name="SchemaRootNode">
		/// The root node of the schema implementing the type.
		/// </param>
		/// <returns>
		/// A type object representing the type referenced by the parameter, or null if the type name is
		/// not supported.
		/// </returns>
        public static AnyType CreateType(string TypeName, XmlNode SchemaRootNode)
        {
            AnyType TypeToReturn;

            switch (TypeName)
            {
                case "token":
                    TypeToReturn = new Token(SchemaRootNode);
                    break;
                case "string":
                    TypeToReturn = new String(SchemaRootNode);
                    break;
                case "xbrli:decimalItemType":
                    TypeToReturn = new DecimalItemType();
                    break;
                case "xbrli:monetaryItemType":
                    TypeToReturn = new MonetaryItemType();
                    break;
                case "xbrli:pureItemType":
                    TypeToReturn = new PureItemType();
                    break;
                case "xbrli:sharesItemType":
                    TypeToReturn = new SharesItemType();
                    break;
                case "xbrli:tokenItemType":
                    TypeToReturn = new TokenItemType();
                    break;
                case "xbrli:stringItemType":
                    TypeToReturn = new StringItemType();
                    break;
                default:
                    TypeToReturn = null;
                    break;
            }
            return TypeToReturn;
        }
    }
}

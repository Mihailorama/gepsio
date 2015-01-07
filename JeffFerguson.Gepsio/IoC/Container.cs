using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio.IoC
{
    /// <summary>
    /// A very simple IoC container.
    /// </summary>
    internal static class Container
    {
        private static Dictionary<Type, Type> registeredTypes;

        static Container()
        {
            registeredTypes = new Dictionary<Type, Type>();
            RegisterSystemXmlTypes();
        }

        private static void RegisterSystemXmlTypes()
        {
            Register<IAttribute, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.Attribute>();
            Register<IAttributeList, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.AttributeList>();
            Register<IDocument, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.Document>();
            Register<INamespaceManager, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.NamespaceManager>();
            Register<INode, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.Node>();
            Register<INodeList, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.NodeList>();
            Register<IQualifiedName, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.QualifiedName>();
            Register<ISchema, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.Schema>();
            Register<ISchemaElement, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.SchemaElement>();
            Register<ISchemaSet, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.SchemaSet>();
            Register<ISchemaType, JeffFerguson.Gepsio.Xml.Implementation.SystemXml.SchemaType>();
        }

        private static void Register<TInterface, TImplementation>()
        {
            registeredTypes.Add(typeof(TInterface), typeof(TImplementation));
        }

        public static TInterface Resolve<TInterface>()
        {
            if (registeredTypes.ContainsKey(typeof(TInterface)) == false)
                throw new KeyNotFoundException("Interface type not registered.");
            Type implementationType = registeredTypes[typeof(TInterface)];
            return (TInterface)Activator.CreateInstance(implementationType);
        }
    }
}

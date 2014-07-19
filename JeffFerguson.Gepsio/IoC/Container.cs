using JeffFerguson.Gepsio.Xml.Implementation.DotNet;
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
            RegisterDotNetTypes();
        }

        private static void RegisterDotNetTypes()
        {
            Register<IAttribute, Attribute>();
            Register<IAttributeList, AttributeList>();
            Register<IDocument, Document>();
            Register<INamespaceManager, NamespaceManager>();
            Register<INode, Node>();
            Register<INodeList, NodeList>();
            Register<IQualifiedName, QualifiedName>();
            Register<ISchema, Schema>();
            Register<ISchemaElement, SchemaElement>();
            Register<ISchemaSet, SchemaSet>();
            Register<ISchemaType, SchemaType>();
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

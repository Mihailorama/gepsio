using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;

namespace JeffFerguson.Gepsio
{
    internal class AssemblyResources
    {
        static ResourceManager staticResourceManager;

        static AssemblyResources()
        {
            string ResourceFile = "JeffFerguson.Gepsio.Gepsio";
            staticResourceManager = new ResourceManager(ResourceFile, typeof(AssemblyResources).Assembly);
        }

        public static string GetName(string Key)
        {
            return staticResourceManager.GetString(Key);
        }
    }
}

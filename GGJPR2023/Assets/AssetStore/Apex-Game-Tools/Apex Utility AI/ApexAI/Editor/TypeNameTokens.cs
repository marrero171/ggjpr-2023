namespace Apex.AI.Editor
{
    using System;
    using System.Text.RegularExpressions;

    public class TypeNameTokens
    {
        public TypeNameTokens(string completeTypeName)
        {
            //Since the format is well know this performs far better than a regex
            var outer = completeTypeName.Split(',');
            this.completeTypeName = string.Concat(outer[0], ",", outer[1]);
            this.fullTypeName = outer[0];
            this.assemblyName = outer[1].Trim();

            var inner = outer[0].Split('.');
            this.simpleTypeName = inner[inner.Length - 1];
        }

        public string completeTypeName { get; private set; }

        public string simpleTypeName { get; private set; }

        public string fullTypeName { get; private set; }

        public string assemblyName { get; private set; }
    }
}

/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor.Reflection
{
    using System.Collections.Generic;
    using System.Linq;

    public class DependencyChecker
    {
        private Dictionary<string, DependencyCheck[]> _dependenciesLookup = new Dictionary<string, DependencyCheck[]>();

        public void Add(string dependent, DependencyCheck[] checks)
        {
            _dependenciesLookup[dependent] = checks;
        }

        public bool AreDependenciesSatisfied(string dependent)
        {
            DependencyCheck[] checks;
            if (_dependenciesLookup.TryGetValue(dependent, out checks))
            {
                return checks.All(c => c.isSatisfied);
            }

            return true;
        }
    }
}

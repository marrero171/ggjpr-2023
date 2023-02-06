/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.AI.Editor.Reflection
{
    using System;
    using System.Reflection;

    public sealed class MemberData
    {
        public MemberInfo member;
        public Type rendererType;
        public string name;
        public string description;
        public string category;
        public int outerSortOrder;
        public int innerSortOrder;
    }
}
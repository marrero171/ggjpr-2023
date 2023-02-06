/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.AI.Editor
{
    using System.Reflection;
    using UnityEngine;

    public sealed class PngResource
    {
        public PngResource(string name, int width, int height)
            : this(name, width, height, SkinMode.None)
        {
        }

        public PngResource(string name, int width, int height, SkinMode skinning)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            this.skinning = skinning;

            this.key = string.Format("{0}_{1}_{2}", name, width, height);
        }

        public int width { get; private set; }

        public int height { get; private set; }

        public string name { get; private set; }

        public string key { get; private set; }

        public SkinMode skinning { get; private set; }

        public Texture2D texture
        {
            get
            {
                return ResourceManager.LoadPngResource(this);
            }
        }
    }
}

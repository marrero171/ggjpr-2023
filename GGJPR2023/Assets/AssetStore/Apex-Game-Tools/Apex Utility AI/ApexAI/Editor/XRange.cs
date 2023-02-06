/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor
{
    public struct XRange
    {
        public float xMin;
        public float xMax;
        public float width;

        public XRange(float x, float width)
        {
            this.xMin = x;
            this.xMax = x + width;
            this.width = width;
        }

        public bool Contains(float xpos)
        {
            return (xpos >= this.xMin) && (xpos < this.xMax);
        }
    }
}

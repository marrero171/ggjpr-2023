/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor
{
    using Apex.Editor.Versioning;

    public static class ProductInfo
    {
        public static readonly IProductIdentifier product;

        static ProductInfo()
        {
            product = typeof(ProductInfo).Assembly.GetAttribute<ApexProductAttribute>(false);
        }
    }
}

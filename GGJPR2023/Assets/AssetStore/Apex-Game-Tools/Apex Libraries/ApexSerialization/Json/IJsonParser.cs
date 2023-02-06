/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Serialization.Json
{
    public interface IJsonParser
    {
        StageElement Parse(string json);
    }
}

using System;

namespace BB.SaveLoadSystem
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class Savable : Attribute
    {
    }
}

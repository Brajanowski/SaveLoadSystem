using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BB.SaveLoadSystem
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class Savable : Attribute
    {
    }
}

﻿using System.Runtime.Serialization;
using UnityEngine;

namespace BB.SaveLoadSystem.Surrogates
{
    public class Vector4Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector4 vector4 = (Vector4)obj;
            info.AddValue("x", vector4.x);
            info.AddValue("y", vector4.y);
            info.AddValue("z", vector4.z);
            info.AddValue("w", vector4.w);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector4 vector4 = (Vector4)obj;
            vector4.x = info.GetSingle("x");
            vector4.y = info.GetSingle("y");
            vector4.z = info.GetSingle("z");
            vector4.w = info.GetSingle("w");
            return vector4;
        }
    }
}

using System.Runtime.Serialization;
using UnityEngine;

namespace BB.SaveLoadSystem.Surrogates
{
    public class Color32Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Color32 color = (Color32)obj;
            info.AddValue("r", color.r);
            info.AddValue("g", color.g);
            info.AddValue("b", color.b);
            info.AddValue("a", color.a);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Color32 color = (Color32)obj;
            color.r = info.GetByte("r");
            color.g = info.GetByte("g");
            color.b = info.GetByte("b");
            color.a = info.GetByte("a");
            return color;
        }
    }
}

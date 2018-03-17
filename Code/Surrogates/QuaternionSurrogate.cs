using System.Runtime.Serialization;
using UnityEngine;

namespace BB.SaveLoadSystem.Surrogates
{
    public class QuaternionSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Quaternion quaternion = (Quaternion)obj;
            info.AddValue("x", quaternion.x);
            info.AddValue("y", quaternion.y);
            info.AddValue("z", quaternion.z);
            info.AddValue("w", quaternion.w);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Quaternion quaternion = (Quaternion)obj;
            quaternion.x = info.GetSingle("x");
            quaternion.y = info.GetSingle("y");
            quaternion.z = info.GetSingle("z");
            quaternion.w = info.GetSingle("w");
            return quaternion;
        }
    }
}

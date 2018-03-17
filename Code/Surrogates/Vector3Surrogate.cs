using System.Runtime.Serialization;
using UnityEngine;

namespace BB.SaveLoadSystem.Surrogates
{
    public class Vector3Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 vector3 = (Vector3)obj;
            info.AddValue("x", vector3.x);
            info.AddValue("y", vector3.y);
            info.AddValue("z", vector3.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 vector3 = (Vector3)obj;
            vector3.x = info.GetSingle("x");
            vector3.y = info.GetSingle("y");
            vector3.z = info.GetSingle("z");
            return vector3;
        }
    }
}

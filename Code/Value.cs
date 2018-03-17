using System;
using UnityEngine;

namespace BB.SaveLoadSystem
{
    [Serializable]
    public class Value
    {
        [SerializeField]
        public Type type;
        public string name;
        public string serializedValue;

        public Value()
        {
            name = "";
        }

        public Value(string _name, object value)
        {
            type = value.GetType();
            name = _name;

            SetValue(value);
        }

        private void SetValue(object value)
        {
            // If it's a C# default type like int or float then we can just cast this value to string.
            if (value.GetType().IsPrimitive)
            {
                serializedValue = value.ToString();
            }
            else
            {
                // Otherwise serialize it to json
                serializedValue = JsonUtility.ToJson(value);
            }
        }
    }
}

using System;
using UnityEngine;

namespace BB.SaveLoadSystem
{
    [Serializable]
    public class Value
    {
        [SerializeField]
        public string type;

        [SerializeField]
        public string name;

        [SerializeField]
        public string value;

        public Value()
        {
            name = "";
        }

        public Value(string _name, object _value)
        {
            type = _value.GetType().FullName;
            name = _name;

            SetValue(_value);
        }

        private void SetValue(object _value)
        {
            // If it's a C# default type like int or float then we can just cast this value to string.
            if (_value.GetType().ToString().StartsWith("System."))
            {
                value = _value.ToString();
            }
            else
            {
                // Otherwise serialize it to json
                value = JsonUtility.ToJson(_value);
            }
        }
    }
}

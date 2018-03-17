using System;

namespace BB.SaveLoadSystem
{
    [Serializable]
    public class ObjectFieldData
    {
        /// <summary>
        /// Object id that field data is stored
        /// </summary>
        public int objectId;

        /// <summary>
        /// Field name
        /// </summary>
        public string fieldName;

        /// <summary>
        /// Field value
        /// </summary>
        public object value;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public ObjectFieldData(int objectId, string fieldName, object value)
        {
            this.objectId = objectId;
            this.fieldName = fieldName;
            this.value = value;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BB.SaveLoadSystem
{
    [Serializable]
    public class PackedData
    {
        [SerializeField]
        public List<ObjectData> objects;

        public void Add(ObjectData objectData)
        {
            if (objects == null)
            {
                objects = new List<ObjectData>();
            }

            objects.Add(objectData);
        }
    }
}
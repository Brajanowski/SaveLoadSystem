﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace BB.SaveLoadSystem
{
    [Serializable]
    public class ObjectData
    {
        [SerializeField]
        public int id;

        [SerializeField]
        public Value value;

        public ObjectData(int id, Value value)
        {
            this.id = id;
            this.value = value;
        }
    }
}
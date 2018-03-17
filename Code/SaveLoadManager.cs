using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BB.SaveLoadSystem
{
    public class SaveLoadManager : MonoBehaviour
    {
        // Singleton
        private static SaveLoadManager _instance;
        public static SaveLoadManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SaveLoadManager>();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Objects to save that are manually added
        /// </summary>
        public List<MonoBehaviour> savables = new List<MonoBehaviour>();

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(_instance.gameObject);
            }
        }

        /// <summary>
        /// Saves current state to a file
        /// </summary>
        /// <param name="fileName">Where we should save? Path to file</param>
        /// <returns></returns>
        public bool Save(string fileName)
        {
            var formatter = new BinaryFormatter();
            var packedData = GenerateData();
            var file = File.Open(fileName, FileMode.Create);
            formatter.Serialize(file, packedData);
            file.Close();
            return true;
        }

        /// <summary>
        /// Loads game state from file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Load(string fileName)
        {
            var formatter = new BinaryFormatter();

            PackedData data = null;
            try
            {
                var file = File.Open(fileName, FileMode.Open);
                data = (PackedData)formatter.Deserialize(file);
                file.Close();
            }
            catch
            {
                return false;
            }

            foreach (var obj in data.objects)
            {
                UpdateObjectData(obj);
            }

            return true;
        }

        private PackedData GenerateData()
        {
            PackedData packedData = new PackedData();

            int id = 0;
            foreach (var savable in savables)
            {
                if (savable != null)
                {
                    var savableField = GetSavableFields(savable);

                    foreach (var field in savableField)
                    {
                        packedData.Add(new ObjectData(id, new Value(field.Name, field.GetValue(savable))));
                    }
                }
                else
                {
                    Debug.Log("There isn't any object assigned, skipping id: " + id);
                }

                ++id;
            }

            return packedData;
        }

        private void UpdateObjectData(ObjectData obj)
        {
            Type type = obj.value.type;
            if (type != null)
            {
                try
                {
                    object value = null;
                    if (obj.value.type.IsPrimitive)
                    {
                        value = TypeDescriptor.GetConverter(type).ConvertFromString(obj.value.serializedValue);
                    }
                    else
                    {
                        value = JsonUtility.FromJson(obj.value.serializedValue, type);
                    }

                    UpdateFieldValue(savables[obj.id], obj.value.name, value);
                }
                catch (Exception e)
                {
                    Debug.LogError("Couldn't parse a value: " + obj.value.name + ", error message: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Couldn't find type: " + obj.value.type);
            }
        }

        private static FieldInfo[] GetSavableFields(object obj)
        {
            var allFields = obj.GetType().GetFields();
            List<FieldInfo> savableFields = new List<FieldInfo>();

            foreach (var field in allFields)
            {
                if (field.IsDefined(typeof(Savable), false))
                {
                    savableFields.Add(field);
                }
            }

            return savableFields.ToArray();
        }

        private static void UpdateFieldValue(object obj, string name, object value)
        {
            // Get all savable fields
            var savableFields = GetSavableFields(obj);

            // Search for desired field and assign new value
            foreach (var field in savableFields)
            {
                if (field.Name == name)
                {
                    field.SetValue(obj, value);
                    // Done
                    return;
                }
            }
        }
    }
}

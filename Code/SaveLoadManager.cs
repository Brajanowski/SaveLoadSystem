using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
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
            var json = GenerateJSON(true);
            Debug.Log(json);

            try
            {
                StreamWriter file = new StreamWriter(fileName);
                file.Write(json);
                file.Close();
            }
            catch (Exception e)
            {
                Debug.Log("Couldn't create file: " + fileName + ", error message: " + e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Loads game state from file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Load(string fileName)
        {
            string json = GetFileContent(fileName);
            if (json != null)
            {
                try
                {
                    PackedData data = JsonUtility.FromJson<PackedData>(json);
                    foreach (var obj in data.objects)
                    {
                        UpdateObjectData(obj);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Couldn't parse a content of file: " + fileName + ", error message: " + e.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private string GetFileContent(string fileName)
        {
            StreamReader file;
            try
            {
                string content = "";

                file = new StreamReader(fileName);
                content = file.ReadToEnd();
                file.Close();

                return content;
            }
            catch (Exception e)
            {
                Debug.LogError("Couldn't load file: " + fileName + ", error message: " + e.Message);
                return null;
            }
        }

        private string GenerateJSON(bool pretty)
        {
            PackedData packedData = new PackedData();

            int id = 0;
            foreach (var savable in savables)
            {
                var savableField = GetSavableFields(savable);

                foreach (var field in savableField)
                {
                    packedData.Add(new ObjectData(id, new Value(field.Name, field.GetValue(savable))));
                }

                ++id;
            }

            return JsonUtility.ToJson(packedData, pretty);
        }

        private void UpdateObjectData(ObjectData obj)
        {
            string typeName = obj.value.type;
            if (typeName.StartsWith("UnityEngine."))
            {
                typeName += ", UnityEngine";
            }

            Type type = Type.GetType(typeName);
            if (type != null)
            {
                if (obj.value.type.StartsWith("System."))
                {
                    try
                    {
                        object value = TypeDescriptor.GetConverter(type).ConvertFromString(obj.value.value);
                        UpdateFieldValue(savables[obj.id], obj.value.name, value);
                    }
                    catch
                    {
                        Debug.LogError("Couldn't parse value: " + obj.value.name);
                    }
                }
                else
                {
                    try
                    {
                        object value = JsonUtility.FromJson(obj.value.value, type);
                        UpdateFieldValue(savables[obj.id], obj.value.name, value);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Couldn't parse a value: " + obj.value.name + ", error message: " + e.Message);
                    }
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

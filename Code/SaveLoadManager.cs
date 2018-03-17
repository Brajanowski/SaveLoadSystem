using BB.SaveLoadSystem.Surrogates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BB.SaveLoadSystem
{
    public class SaveLoadManager : MonoBehaviour
    {
        // Singleton
        private static SaveLoadManager _instance;

        /// <summary>
        /// Get singleton instance to this object
        /// </summary>
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

        /// <summary>
        /// We use binary serialization
        /// </summary>
        private BinaryFormatter _formatter;

        /// <summary>
        /// Contains surrogates for unity types like Vector or Color
        /// </summary>
        private SurrogateSelector _surrogateSelector;

        private void Awake()
        {
            // Singleton
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(_instance.gameObject);
            }

            CreateBinaryFormatter();
        }

        private void CreateBinaryFormatter()
        {
            _formatter = new BinaryFormatter();
            _surrogateSelector = new SurrogateSelector();
            _surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), new Vector2Surrogate());
            _surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3Surrogate());
            _surrogateSelector.AddSurrogate(typeof(Vector4), new StreamingContext(StreamingContextStates.All), new Vector4Surrogate());
            _surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), new QuaternionSurrogate());
            _surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), new ColorSurrogate());
            _surrogateSelector.AddSurrogate(typeof(Color32), new StreamingContext(StreamingContextStates.All), new Color32Surrogate());

            _formatter.SurrogateSelector = _surrogateSelector;
        }

        /// <summary>
        /// Saves current state to a file
        /// </summary>
        /// <param name="fileName">Where we should save? Path to file</param>
        /// <returns></returns>
        public bool Save(string fileName)
        {
            // Generate data
            var packedData = GenerateData();

            // Save generated data
            using (var file = File.Open(fileName, FileMode.Create))
            {
                _formatter.Serialize(file, packedData);
                file.Close();
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
            // Load and deserialize data from file
            List<ObjectFieldData> data = null;
            try
            {
                using (var file = File.Open(fileName, FileMode.Open))
                {
                    data = (List<ObjectFieldData>)_formatter.Deserialize(file);
                    file.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error: " + e.Message);
                return false;
            }

            // Now we can use this data to restore fields values
            if (data != null)
            {
                foreach (var obj in data)
                {
                    UpdateFieldValue(savables[obj.objectId], obj.fieldName, obj.value);
                }
            }

            return true;
        }

        /// <summary>
        /// Generate packed data ready to serialize and save.
        /// </summary>
        /// <returns>Generated data</returns>
        private List<ObjectFieldData> GenerateData()
        {
            List<ObjectFieldData> packedData = new List<ObjectFieldData>();

            int id = 0;
            foreach (var savable in savables)
            {
                if (savable != null)
                {
                    var savableField = GetSavableFields(savable);

                    foreach (var field in savableField)
                    {
                        packedData.Add(new ObjectFieldData(id, field.Name, field.GetValue(savable)));
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

        /// <summary>
        /// Get and return all fields that are marked as "Savable"
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static FieldInfo[] GetSavableFields(object obj)
        {
            var allFields = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
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

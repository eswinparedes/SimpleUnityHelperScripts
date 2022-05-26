using SUHScripts.Functional;
using System;
using UnityEngine;

namespace SUHScripts.SaveSystem
{
    public class SaveValue<T>
    {
        public SaveValue(ISaveSystem saveSystem, T defaultValue, string key, string filePath)
        {
            m_saveSystem = saveSystem;

            if (m_saveSystem.FileExists(filePath))
            {
                var topt = m_saveSystem.Load<T>(key, filePath);

                if (!topt.IsSome)
                {
                    saveSystem.Save<T>(defaultValue, key, filePath);
                    m_value = defaultValue;
                }
                else
                {
                    m_value = topt.Value;
                }
            }
            else
            {
                saveSystem.Save<T>(defaultValue, key, filePath);
                m_value = defaultValue;
            }

            m_fileName = filePath;
            m_key = key;
        }

        ISaveSystem m_saveSystem;
        string m_fileName = null;
        string m_key = null;
        T m_value;

        public T GetValue(bool doLoadFromFile)
        {
            if (doLoadFromFile)
            {
                var topt = m_saveSystem.Load<T>(m_key, m_fileName);

                if (!topt.IsSome)
                    throw new Exception($"Save System unable to save value of type {typeof(T).Name}");

                m_value = topt.Value;
                return m_value;
            }
            else
            {
                return m_value;
            }
        }

        public void SetValue(bool doSaveToFile, T value)
        {
            //Debug.Log($"Saving {value} to key {m_key} at file {m_fileName}");

            m_value = value;

            if (doSaveToFile)
            {
                m_saveSystem.Save<T>(m_value, m_key, m_fileName);
            }

            
        }
    }

}


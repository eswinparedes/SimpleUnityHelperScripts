using System.Collections.Generic;
using UniRx;
using UnityEngine;

public static class StaticIDMap<TValue>
{
    static int m_currentIndex = int.MinValue;
    static ReactiveDictionary<int, TValue> m_map = new ReactiveDictionary<int, TValue>();
    static ReactiveDictionary<TValue, int> m_mapInvert = new ReactiveDictionary<TValue, int>();
    public static IReadOnlyReactiveDictionary<int, TValue> Map => m_map;
    public static IReadOnlyReactiveDictionary<TValue, int> MapInvert => m_mapInvert;
    public static int Register(TValue target)
    {
        if (m_mapInvert.ContainsKey(target))
        {
            return m_mapInvert[target];
        }

        var index = ++m_currentIndex;
        m_map.Add(index, target);
        m_mapInvert.Add(target, index);
        return index;
    }

    /// <summary>
    /// Returns true if target was removed, false if it was not in the map
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool Remove(TValue target)
    {
        if (m_mapInvert.ContainsKey(target))
        {
            var id = m_mapInvert[target];
            m_map.Remove(id);
            m_mapInvert.Remove(target);
            return true;
        }

        Debug.LogError("Target is not is map");
        return false;
    }

    public static bool Push(TValue target, int targetIndex)
    {
        if (m_map.ContainsKey(targetIndex))
        {
            Debug.LogError($"Target index {targetIndex} already in StaticIDMap {typeof(TValue).Name}");
            return false;
        }

        m_map.Add(targetIndex, target);
        m_mapInvert.Add(target, targetIndex);
        return true;
    }
}

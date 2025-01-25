using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCollection", menuName = "Scriptable Objects/LevelCollection")]
public class LevelCollection : ScriptableObject
{
    [System.Serializable]
    public struct LevelData
    {
        public LevelID id;
        public GameObject prefab;
    }

    [SerializeField] private List<LevelData> levels;

    public GameObject GetLevel(LevelID levelID)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].id == levelID) return levels[i].prefab;
        }
        return null;
    }
    public GameObject GetRandomLevel()
    {
        return levels[Random.Range(0, levels.Count)].prefab;
    }
}

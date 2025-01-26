using UnityEditor;
using UnityEngine;

public class LevelManager
{
    public Level curLevel;
    public void LoadLevel(GameObject levelObject)
    {
        levelObject = GameObject.Instantiate(levelObject, Vector3.zero, Quaternion.identity);

        curLevel = levelObject.GetComponent<Level>();
    }
    public void UnloadLevel()
    {
        GameObject.Destroy(curLevel.gameObject);
    }
    public void Update()
    {
        if (curLevel == null) return;
    }
}

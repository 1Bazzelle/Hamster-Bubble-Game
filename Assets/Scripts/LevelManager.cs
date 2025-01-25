using UnityEngine;

public class LevelManager
{
    public LevelID selectedLevel;

    public Level curLevel;
    public void LoadSelectedLevel()
    {
        GameObject levelObject = GameObject.Instantiate(GameManager.Instance.levelCollection.GetLevel(selectedLevel));
        
        levelObject.transform.position = Vector3.zero;
        curLevel = levelObject.GetComponent<Level>();
    }

    public void Update()
    {
        if (curLevel == null) return;


    }
}
